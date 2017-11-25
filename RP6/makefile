MCU   = atmega32

CFLAGS = -mmcu=$(MCU)  -Os -Wall -std=gnu99 
CC = avr-gcc
AVRDUDE = avrdude
PROGRAMMER_ARGS_AR = -b 115200 -P /dev/tty.usbmodem1411 -D
PROGRAMMER_TYPE_AR = stk500v2

SRC= main.c

OBJ= $(SRC:.c=.o)
TARGET = main

OBJCOPY = avr-objcopy
FORMAT  = ihex

all:elf hex

elf: $(TARGET).elf
hex: $(TARGET).hex


$(TARGET).elf: $(OBJ)
	$(CC)  $(CFLAGS) $^ -o $@ 

%.hex: %.elf
	$(OBJCOPY) -O $(FORMAT) -R .eeprom $< $@


%.o:%.c motor.h
	$(CC)  $(CFLAGS) -c -Os $<


clean:
	rm -f *.o

flash:$(TARGET).hex
	$(AVRDUDE) -c $(PROGRAMMER_TYPE_AR) -p $(MCU) $(PROGRAMMER_ARGS_AR) -U flash:w:$<	

program:flash

bib:motor.o 
	avr-ar rcs motor.o
	
