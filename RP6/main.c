#define F_CPU		8000000
#define SCL_freq 	100000
#define BAUDRATE	38400  
#define UBRR_BAUD	(((long)F_CPU/((long)16 * BAUDRATE))-1)

#include <avr/io.h>
#include <avr/interrupt.h>
#include <util/twi.h> 
#include <stdlib.h>			// needed for default functions (atoi)
#include <util/delay.h>		// for the _ms_delay() function

#include "RP6_pins.h"

#
#define MAX_SPEED 100
#define MAX_SPEED_BACKWARDS -100
#define MIN_SPEED 0
#define DELTA_SPEED 3
#define AUTONOOM_SPEED 60

#define resetData() for (uint8_t i = 0; i < 8; i++) data_array[i] = 0

volatile char twi_data[8]; //data array for ISR/TWI
volatile uint8_t twi_datacount = 0; //counter for data array ISR/TWI 
char data_array[8]; //seperate data array to not corrupt or interrupt ISR data array

void (*twi_receive) ();
void (*twi_send) ();
void i2c_init_slave(uint8_t address);
void i2c_stop_slave();
void onReceive( void (*receiveEvent) ());
void onSend( void (*sendEvent) ());

void initUSART();
void writeChar(char ch);
void writeString(char *string);
void writeInteger(int16_t number, uint8_t base);

void motorStop(); // stops everything
void motorSetSpeed(int left, int right); // set the motor speed, can be negative
void motorSharpTurn(char dir); // makes a sharp turn, give a 'l' or a 'r' to go left or right
void motorInit(); // initialization, needs to run once at the start
void motorBumperHit(char bumperDir); // checks if the bumper got hit
void motorBuildSpeed(int startSpeed, int endSpeed); // builds speed to a certain threshold
void motorCommandConvert(char *command); // converting the commands received via I2C to functions
void motorLowerSpeed (int startSpeed, int endSpeed); // Slowly lowers the speed
void motorSetSpeedLeft(int speed); // adjust the left speed
void motorSetSpeedright(int speed); // adjust the right speed

ISR(TIMER0_OVF_vect) {
	//check if bumpers were hit
	if(PINC & SL3){ // Right bumper
		motorBumperHit('R');
	} else if (PINB & SL6) { // left bumper
		motorBumperHit('L');
	}
}

ISR(TWI_vect){
	static char twi_data[5]; //data array for ISR/TWI
	static uint8_t twi_datacount = 0; //counter for data array ISR/TWI 

	switch(TWSR){
		// Write to slave received, ACK returned, 0x60
		case TW_SR_SLA_ACK:
			twi_datacount = 0;
			break;

 		// data received, ACK returned, 0x80
		case TW_SR_DATA_ACK:
			twi_data[twi_datacount++] = TWDR;
 			break;

		// stop or repeated start condition received while selected, 0xA0
		case TW_SR_STOP:
			twi_data[twi_datacount] = 0; //null terminate string
			twi_datacount = 0; //reset count
			//Copy TWI Data array to new array to preserve original
			for (int i = 0; i < 5; i++) {
				data_array[i] = twi_data[i];
			}
			//strncpy(data_array, twi_data, 4);
			twi_receive();
		  	resetData();
			break;

		default:
			break;
	}	

	TWCR |= (1<<TWINT);		//clear TWI Interrupt Flag
}

void executeI2CCommand(){
	motorCommandConvert(data_array);
}

int main(){
	initUSART();

	i2c_init_slave(0xFF);

	motorInit();

	onReceive(executeI2CCommand);

	sei();

	while(1);
}
void initUSART() {

	  UBRRH = UBRR_BAUD >> 8;
	  UBRRL = (uint8_t) UBRR_BAUD;
	  UCSRA = 0x00;
	  UCSRC = (1<<URSEL)|(1<<UCSZ1)|(1<<UCSZ0);
	  UCSRB = (1 << TXEN) | (1 << RXEN);
}

void writeChar(char ch)
{
	while (!(UCSRA & (1<<UDRE)));
	UDR = (uint8_t)ch;
}

void writeString(char *string)
{
	while(*string)
	writeChar(*string++);
}

void writeInteger(int16_t number, uint8_t base)
{
	char buffer[17];
	itoa(number, &buffer[0], base);
	writeString(&buffer[0]);
}

void i2c_init_slave(uint8_t address){
	//Reset all bits in TWSR
 	TWSR = 0;
 	//Set bit rate
 	TWBR = ((F_CPU / SCL_freq) - 16) / 2;
 	//Enable TWI Interrupt, Enable TWI, Enable Acknowledgement
 	TWCR = (1 << TWIE) | (1 << TWEN) | (1 << TWEA);
 	//Set device address
 	TWAR = address << 1;
}

void i2c_stop_slave(){
	//clear TWI bit, TWI Acknowledgement bit
	TWCR &= ~((1 << TWEN) | (1 << TWEA) );
}

void onReceive( void (*receiveEvent) ()) {
	//Any function can be passed as argument
	//which will be executed when twi_receive is called
	twi_receive = receiveEvent;
}

void onSend( void (*sendEvent) ()) {
	//Any function can be passed as argument
	//which will be executed when twi_send is called
	twi_send = sendEvent;
}

void motorCommandConvert(char command[]) {
	/*
		converting command recieved from I2C
		
		L = left
		R = right
		M = master (left & right)
		B = backwards (always master)
		A = autonomous driving
		
		ex:
		'L50' = the left wheel needs to be set at 50% speed
	*/
	
	// extracting the the prefix (L,R,M,B or A) from the string
	char prefix = command[0];
	
	// extracting the speed (0-100) from the string
	int speed = atoi(command + 1);
	
	writeChar(prefix);
	writeInteger(speed, 10);
	writeChar('\n');
	writeChar('\r');

	// logic deciding what to do with what command
	switch(prefix) {
		case 'L': motorSetSpeed(speed, -30); break;		// setting the left speed
		case 'R': motorSetSpeed(-30, speed); break;		// setting the right speed
		case 'M': motorSetSpeed(speed, speed); break;	// setting the master speed
		case 'B': motorSetSpeed(speed*-1, speed*-1); break;	// set the backwards speed by multiplying the speed with -1
		case 'A': motorSetSpeed(AUTONOOM_SPEED, AUTONOOM_SPEED); break;	// setting the autonomous speed
		case 'Q': motorStop(); break;
		case 'U': motorBumperHit('R'); break;
	}
}

// checking if the bumbers are hit
void motorBumperHit(char bumperDir) {
	
	// the right bumper
	if(bumperDir == 'R'){
		// drive backwards for a second
		motorSetSpeed(-50, -50);
		for (int i=0; i<4; i++) _delay_ms(250);
		
		// make a sharp turn to the left
		motorSharpTurn('l');
		
		// move forwards
		motorSetSpeed(50,50);
		
		// the left bumper
	} else if (bumperDir == 'L') {
		// drive backwards for a second
		motorSetSpeed(-50, -50);
		for (int i=0; i<4; i++) _delay_ms(250);
		
		// make a sharp turn to the right
		motorSharpTurn('r');
		
		// move forwards
		motorSetSpeed(50,50);	
	}
}

// initialization, this should be the first line and the first function in the main
void motorInit(){
	
	/* 
	*	The top of the counter, the counter counts to here and than counts down
	*	We set this to 100 so we can send a percentage, makes it easier
	*
	*	MAX_SPEED = 100
	*/
	ICR1  = MAX_SPEED; 
	
	
	//	Setting the registers for phase correct PWM, ICR1 as TOP
	TCCR1A |= (0 << WGM10) | (1 << WGM11) | (1 << COM1A1) | (1 << COM1B1);
	TCCR1B |=  (1 << WGM13) | (0 << WGM12) | (1 << CS10);
	
	// prescaler of 1024 for timer0
	TCCR0 |= (1 << CS02) | (0 << CS01) | (1 << CS00);
	// enabling the timer0 overflow interrupt
	TIMSK |= (1 << TOIE0);

	// Set counter on 0
	TCNT0 = 0;
	
	
	/* 
	*	Setting the registers as output
	*	MOTOR is the motor, _r is the right motor, _l is the left motor
	*
	*	SL6 & SL3 are the bumpers
	*/
	DDRD = MOTOR_R | MOTOR_L;
	DDRC = DIR_R | DIR_L;
	
	/*
	*	poorten van de bumpers open zetten
	*/
	PORTB &= ~SL6;
	PORTC &= ~SL3;
	
	DDRB &= ~SL6;
	DDRC &= ~SL3;
	
}

// Setting the motor speed
void motorSetSpeed (int left, int right) {
	
	// stopping everything
	motorStop();
	
	// checking if the speed must be backwards
	if (left < 0) {
		// setting the speed backwards
		PORTC |= DIR_L;
		
		// making the speed readable for the OCR register (positive)
		left *= -1;
	}
	
	if (right < 0) {
		// setting the speed backwards
		PORTC |= DIR_R;
		
		// making the speed readable for the OCR register (positive)
		right *= -1;
	}
	
	// The register for the left wheel, this is the duty cycle
	OCR1B = left;
	// The register for the right wheel, this is the duty cycle
	OCR1A = right;
}

// setting the speed of the left wheel
void motorSetSpeedLeft (int speed) {
	// stopping everything
	motorStop();
	
	// checking if the speed must be backwards
	if (speed < 0) {
		// setting the speed backwards
		PORTC |= DIR_L;
		
		// making the speed readable for the OCR register (positive)
		speed *= -1;
	}
	
	// The register for the left wheel, this is the duty cycle
	OCR1B = speed;
}

// setting the speed of the right speed
void motorSetSpeedright (int speed) {
	// stopping everything
	motorStop();
	
	if (speed < 0) {
		// setting the speed backwards
		PORTC |= DIR_R;
		
		// making the speed readable for the OCR register (positive)
		speed *= -1;
	}
	
	// The register for the right wheel, this is the duty cycle
	OCR1A = speed;
}


// stopping everything
void motorStop () {
	// stopping the direction
 	PORTC &= ~(DIR_R | DIR_L);
	// stopping the engine
	PORTD &= ~(MOTOR_R | MOTOR_L);

	// setting the speed at 0, we cant use the setspeed(0,0) funtion for this because setspeed useses the motorstop funcion, it wil get stuck in a infinite loop
	OCR1A = 0;
	OCR1B = 0;
}
	
//	making a sharp turn, 'dir' = 'l' or 'r' or left or right respectively. not case sensetive
void motorSharpTurn (char dir) {
	
	// logic for deciding the direction
	if (dir == 'l' || dir == 'L') {	// left
		motorSetSpeed(-30, 30);	// left stronger than right, so it makes a turn
		for (int i = 0; i < 4; i++) _delay_ms(250); // turning for a second
	} else if (dir == 'r' || dir == 'R'){	// right
		motorSetSpeed(30, -30);	// right stronger than left, so it makes a turn
		for (int i = 0; i < 4; i++) _delay_ms(250); // turnign for a second
	}
}

// slowly builing speed
void motorBuildSpeed (int startSpeed, int endSpeed) {
	// logic deciding if the function can be executed, or that it needs to be adjusted
	if (endSpeed > MAX_SPEED) endSpeed = MAX_SPEED;
	if (startSpeed > endSpeed) motorLowerSpeed(startSpeed, endSpeed);
	if (startSpeed < MIN_SPEED) startSpeed = MIN_SPEED;
	
	// the loop, slowly building speed
	for (int i = startSpeed; i<endSpeed; i+=DELTA_SPEED) {
		motorSetSpeed(i, i);
		_delay_ms(50); // from 0 to 10 this is a 500 ms delay
	}
}

void motorLowerSpeed (int startSpeed, int endSpeed) {
	// logic deciding if the function can be executed, or that it needs to be adjusted
	if (endSpeed < MIN_SPEED) endSpeed = MIN_SPEED;
	if (endSpeed > startSpeed) motorBuildSpeed(startSpeed, endSpeed);
	if (startSpeed > MAX_SPEED) startSpeed = MAX_SPEED;
	
	// the loop, slowly decreasing speed
	for (int i = startSpeed; i>endSpeed; i-=DELTA_SPEED) {
		motorSetSpeed(i,i);
		_delay_ms(50); // from 100 to 0 this is a 500ms delay
	}
}