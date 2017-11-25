#define F_CPU 16000000
#define SCL_frequentie 100000

#define ARR_SIZE 4
#define ULTRA_PIN PB0
#define SLAVE_ADDR 0xFF
#define COMPASS_ADDR 0x60
#define MAX_DISTANCE_CM 10
#define MIN_DISTANCE_CM 45

#include <avr/io.h>
#include <stdlib.h>
#include <util/twi.h>
#include <util/delay.h>
#include <avr/interrupt.h>

void initUSART(void);
void initTimer1(void);
void init_master(void);
uint8_t sendPulse(void);
void sendCompassData(void);
void sendToSlave(uint8_t slave_address, uint8_t data);
void requestFromSlave(uint8_t slave_address, uint8_t maxs);
void sendToSlaveString(uint8_t slave_address, char data[], int len);


uint8_t compass_data[10];
char number_holder[ARR_SIZE];

volatile char state = 'N';
volatile uint8_t index = 0;

ISR(USART0_RX_vect)
{
  while (1)
  {
    //Wait till the register is ready to receive data again
    while (~UCSR0A & (1 << RXC0));
    number_holder[index] = UDR0;

    //Set the mode to autonous if the first index equals to A
    state = (number_holder[0] == 'A') ? 'A' : 'N';

    //if the end of the data is reached then break out of the while and increment variable index
    if (number_holder[index] == '\n') break;
    index++;
  }
  number_holder[index] = 0;

  //reset variable to 0
  index = 0;

  //Send the information that was receiced from the computer to the RP6 via I2C
  sendToSlaveString(SLAVE_ADDR, number_holder, 3);
}

int main(void)
{
  //Set the pins on output
  PORTD = 0x03;

  //Init the usart functionalitity
  initUSART();

  //Initialize the I2C functionalitity
  init_master();

  //Initialize timer1
  initTimer1();

  //Set the global interrupt on.
  sei();

  while (1)
  {
    //get the distance from the ultrasonic sensor
    uint8_t distanceInCM = sendPulse();

    //If the distance is lower or equal to ten
    if (distanceInCM <= MAX_DISTANCE_CM) {
      //send data through I2C
      sendToSlaveString(SLAVE_ADDR, "U", 1);
    }
    else {
      //If the robot is driving autonomous
      if (state == 'A') {
        //send compassdata to the GUI
        sendCompassData();

        //Get distance from the ultrasonic sensor
        distanceInCM = sendPulse();

        if (distanceInCM <= MIN_DISTANCE_CM)
        {
         sendToSlaveString(SLAVE_ADDR, "L50", 3);
        }
        else sendToSlaveString(SLAVE_ADDR, "M50", 3);
      }
    }

    _delay_ms(50);
  }
  return 0;
}

/**
 * sendCompassData(void)
 * 
 * this function gets the compass position and sends it over serial 
 * to the GUI.
 * @param none
 * @return none
 */
void sendCompassData(void)
{
  //Declare compass so that is can store the compass value
  uint16_t compass;

  //Declare two bytes so that they can be used to bitshift
  byte first_byte, second_byte;

  //tell the compass to send two bytes
  sendToSlave(COMPASS_ADDR, 2);

  //request two bytes from the compass
  requestFromSlave(COMPASS_ADDR, 2);

  // merge the recieved bytes to a whole number
  first_byte = compass_data[0];
  compass = first_byte;
  compass <<= 8;
  second_byte = compass_data[1];
  compass += second_byte;

  //write through serial the compass data for the GUI
  //The data is divided by 10 because we recieve 3600 instead of 360, 1800 instead of 180 etc
  writeInt(compass / 10);
  writeChar('\n');
}

/**
 * init_master(void)
 * 
 * this function initializes the arduino as a master
 * 
 * @param none
 * @return none
 */
void init_master(void)
{
  //Set the TWI status register to zero
  TWSR = 0;

  //Set bitrate
  TWBR = ((F_CPU / SCL_frequentie) - 16) / 2;

  //Enable TWI
  TWCR = (1 << TWEN);
}

/**
 * sendToSlave(uint8_t slave_address, uint8_t data)
 * 
 * this function sends data to the slave 
 * 
 * @param uint8_t slave_address e.g. 0xFF
 * @param uint8_t data
 * @return none
 */
void sendToSlave(uint8_t slave_address, uint8_t data)
{
  //Become the master
  TWCR |= (1 << TWSTA);

  while (!(TWCR & (1 << TWINT)));

  TWDR = (slave_address << 1);
  TWCR = (1 << TWINT) | (1 << TWEN);

  while (!(TWCR & (1 << TWINT)));

  TWDR = data;
  TWCR = (1 << TWINT) | (1 << TWEN);

  while (!(TWCR & (1 << TWINT)));

  TWCR = (1 << TWINT) | (1 << TWSTO) | (1 << TWEN);
}

/**
 * sendToSlaveString(uint8_t slave_address, char data[], int len)
 * 
 * this function sends data to the slave 
 * 
 * @param uint8_t slave_address e.g. 0xFF
 * @param char data[] e.g. "M100"  int len
 * @param int len give the length of the string you send
 * @return none
 */
void sendToSlaveString(uint8_t slave_address, char data[], int len)
{
  TWCR |= (1 << TWSTA);
  while (!(TWCR & (1 << TWINT)));

  TWDR = (slave_address << 1);
  TWCR = (1 << TWINT) | (1 << TWEN);

  uint8_t i;
  for (i = 0; i <= len; i++) {
    while (!(TWCR & (1 << TWINT)));

    TWDR = data[i];
    TWCR = (1 << TWINT) | (1 << TWEN);
  }

  TWCR = (1 << TWINT) | (1 << TWSTO) | (1 << TWEN);
}

/**
 * requestFromSlave(uint8_t slave_address, uint8_t maxs)
 * 
 * this function requests data from the slave 
 * 
 * @param uint8_t slave_address e.g. 0xFF
 * @param uint8_t maxs amount of bytes to request
 * @return none
 */
void requestFromSlave(uint8_t slave_address, uint8_t maxs)
{
  uint8_t op[15];

  //Start the I2C bus
  TWCR |= (1 << TWSTA);

  //Wait until the buffer is empty of the I2C bus
  while (!(TWCR & (1 << TWINT)));

  //Read the status of I2C
  op[0] = TWSR;

  //Open the connection to the slave address
  TWDR = (slave_address << 1) + 1;

  //Clear the flags
  TWCR = (1 << TWINT) | (1 << TWEN);

  //Wait until the buffer is empty of the I2C bus
  while (!(TWCR & (1 << TWINT)));

  //Read the status of I2C
  op[1] = TWSR;

  //Read the received data from I2c
  compass_data[0] = TWDR;

  uint8_t tel = 0;
  do
  {
    if (tel == maxs - 1) {
      TWCR = (1 << TWINT) | (1 << TWEN);
    }
    else {
      TWCR = (1 << TWINT) | (1 << TWEN) | (1 << TWEA);
    }

    //Wait until the buffer is empty of the I2C bus
    while (!(TWCR & (1 << TWINT)));

    //Read the status of I2C
    op[tel] = TWSR;

    //Read the received data from I2c
    compass_data[tel] = TWDR;

  } while (op[tel++] == 0x50);

  TWCR = (1 << TWINT) | (1 << TWSTO) | (1 << TWEN);
}

/**
 * initUSART(void)
 * 
 * This function initializes the USART functions
 * 
 * @param none
 * @return none
 */
void initUSART(void)
{
  UBRR0H = 00;

  //baudrade 9600 bij 16MHZ
  UBRR0L = 103;

  //enable recieve and trasmit
  UCSR0B = (1 << RXCIE0) | (1 << RXEN0) | (1 << TXEN0);

  //8 data bits and 1 stop bit
  UCSR0C = (1 << UCSZ01) | (1 << UCSZ00);
}

/**
 * initTimer1(void)
 * 
 * This function initializes the timer1 clock
 * 
 * @param none
 * @return none
 */
void initTimer1(void)
{
  //Clock normal mode
  TCCR1A = 0;
  //No prescaler selected (Clock is off)
  TCCR1B = 0;
}

/**
 * sendPulse(void)
 * 
 * This function gets the distance to a object through the ultrasonic sensor
 * 
 * @param none
 * @return uint8_t distance in centimeters
 */
uint8_t sendPulse(void)
{
  //Declare uint8_t variable cm which will hold the distance in centimeters
  uint8_t cm;

  //Set the pin of the ultrasonic on output
  DDRB  |= (1 << ULTRA_PIN);

  //Send a low pulse
  PORTB &= ~(1 << ULTRA_PIN);

  //Wait 25 milliseconds
  _delay_ms(25);

  //Send a high pulse
  PORTB |= (1 << ULTRA_PIN);

  //Wait 25 milliseconds
  _delay_ms(25);

  //Send a low pulse
  PORTB &= ~(1 << ULTRA_PIN);

  //Set the ultra pin on INPUT
  DDRB  &= ~(1 << ULTRA_PIN);

  //Set the timercount on zero
  TCNT1 = 0;

  //wait if ready
  while (~PINB & (1 << ULTRA_PIN));

  //Listen for a echo
  while ((PINB & (1 << ULTRA_PIN)))
    TCCR1B = 1 << CS11; //Start the timer with a prescaler 8

  //Turn the clock off
  TCCR1B = 0;

  //Calculate the centimeters
  cm = TCNT1 / 116;

  //return the centimeters
  return cm;
}

/**
 * writeString(char st[])
 * 
 * This function writes strings to serial
 * 
 * @param char array
 * @return none
 */
void writeString(char st[])
{
  //this function receives a array of chars
  //The for loop keeps looping till it reaches the end of the array
  uint8_t i;
  for (i = 0 ; st[i] != 0 ; i++) {
    //Every loop it calls the function writeChar and gives the function a char from the array
    writeChar(st[i]);
  }
}

/**
 * writeInt(uint16_t i)
 * 
 * This function writes integers to serial
 * 
 * @param uint16_t i
 * @return none
 */
void writeInt(uint16_t i)
{
  char buffer[16];
  itoa(i, buffer, 10);
  writeString(buffer);
}

/**
 * writeChar(char c)
 * 
 * This function writes a single char to serial
 * 
 * @param char c
 * @return none
 */
void writeChar(char c)
{
  /*when the register is ready to send data again the variable that was given to this function will be set in the
    UDR0 register so it can be sent over Serial */
  while ((UCSR0A & (1 << UDRE0)) == 0);
  UDR0 = c;
}

/**
 * readChar()
 * 
 * This function reads a single char from serial
 * 
 * @param none
 * @return char
 */
char readChar(void)
{
  //This funtion reads a char from the user input
  //The while waits till the register is ready to receive data
  while ((UCSR0A & (1 << UDRE0)) == 0);
  return UDR0;
}
