/*
  Arduino Sketch v0.5
	
  This sketch was adapted for LCARS Home Automation System.

  The Original version was written for "Home Automation using Raspberry Pi 2 and Window 10 IoT"
  Refer this link: https://www.hackster.io/AnuragVasanwala/home-automation

  This sketch is tested on Atmega328p only.

  This sketch prepares an Arduino device as slave device on I2C bus operated by
  Raspberry Pi 2 running Windows 10 IoT Core.

  Objectives:
    + Periodically collect sensor data (Function: loop)
    + OnRecevive, collect 3-bytes mode instruction
      and performa operation based upon it. (Function: ReceiveData)
    + OnRequest, send 14-bytes response array based
      on selected mode by OnReceive. (Function: SendData)
  
  This sketch is provided as it is without any WARRANTY. You can use it for personal as well as
  commercial use.

  I am not liable for any loss of data or injuries caused by this sketch.
*/

#include <Wire.h>  
/* #define _DEBUG_ */

/* Arduino's I2C Slave Address */
#define SLAVE_ADDRESS 0x40

/* PIN DECLARATION */
int Pin_AmbientLight_LDR = A0;
int Pin_PassiveIR = 2; 
int Pin_Temperature = A1;

/* Global Variable */
volatile short Value_AmbientLight_LDR, Value_Temperature;
volatile bool Value_PassiveIR;

/* Protocol Variable */
byte Mode, Pin, Value;
byte Response[14];


void setup()
{
  // Initialize pins
    pinMode(Pin_AmbientLight_LDR, INPUT);
    pinMode(Pin_PassiveIR, INPUT);
    pinMode(Pin_Temperature, INPUT);
    pinMode(0, OUTPUT);
    pinMode(1, OUTPUT);
    pinMode(3, OUTPUT);
    pinMode(4, OUTPUT);
    pinMode(5, OUTPUT);
    pinMode(6, OUTPUT);
    pinMode(7, OUTPUT);
    pinMode(8, OUTPUT);
    pinMode(9, OUTPUT);
    pinMode(10, OUTPUT);
    pinMode(11, OUTPUT);
    pinMode(12, OUTPUT);
    pinMode(13, OUTPUT);
    pinMode(A2, OUTPUT);
    pinMode(A3, OUTPUT);

#ifdef _DEBUG_
    Serial.begin(9600);
#endif
  
  // Initialize I2C Slave on address 'SLAVE_ADDRESS'
    Wire.begin(SLAVE_ADDRESS);
    Wire.onRequest(SendData);
    Wire.onReceive(ReceiveData);
}

void loop()
{
  // Read LDR
  //         Arduino supports 10-bit Analog Read.
  //             Thus we need to convert it into 8-bit.
  Value_AmbientLight_LDR = analogRead(Pin_AmbientLight_LDR);
  Value_AmbientLight_LDR = map(Value_AmbientLight_LDR, 0, 1023, 0, 255);
  
  // Read PassiveIR value
  Value_PassiveIR = (digitalRead(Pin_PassiveIR) == HIGH) ? true : false;

  // Read Temperature Sensor and Convert Voltage into Celsius
  Value_Temperature = (short)((float)(analogRead(Pin_Temperature) * 0.48828125));
  
  // Wait for 100 ms
  delay(100);
}

// Callback for I2C Received Data
void ReceiveData(int byteCount)
{
  // Read first byte which is Protocol Mode
  Mode = Wire.read();

  // Read second byte which is Pin. Only Valid for Mode 2
  Pin = Wire.read();

  // Read third byte which is Pin-Value. Only Valid for Mode 2
  Value = Wire.read();

  // Signal specified pin if Mode 2 is received
  if (Mode == 2)
  {
      digitalWrite(Pin, Value);
  }

#ifdef _DEBUG_
    Serial.print(Mode);
    Serial.print(" ");
    Serial.print(Pin);
    Serial.print(" ");
    Serial.println(Value);
#endif
}

void SendData()
{
    switch (Mode)
    {
        case 0: // Mode: Read Sensor
            Response[0] = (byte)Value_AmbientLight_LDR;

            // Value_PassiveIR is boolean so that we need to convert it into byte
            Response[1] = (byte)((Value_PassiveIR == true) ? 1 : 0);
            
            // Response[2] byte is Sign byte for Temperature
            //         0 - -ve Temperature
            //         1 - +ve Temperature
            Response[2] = (byte)((Value_Temperature < 0) ? 0 : 1);
      
            Serial.println(Value_Temperature);
            
            // -ve Temperature can't be sent in byte. Convert it into +ve equivalent
            Response[3] = (byte)((Value_Temperature < 0) ? (Value_Temperature*(-1)) : Value_Temperature);
            break;
        case 1: // Mode: Read Devices State
            Response[0] = (digitalRead(0) == HIGH) ? 1 : 0;
            Response[1] = (digitalRead(1) == HIGH) ? 1 : 0;
            Response[2] = (digitalRead(3) == HIGH) ? 1 : 0;
            Response[3] = (digitalRead(4) == HIGH) ? 1 : 0;
            Response[4] = (digitalRead(5) == HIGH) ? 1 : 0;
            Response[5] = (digitalRead(6) == HIGH) ? 1 : 0;
            Response[6] = (digitalRead(7) == HIGH) ? 1 : 0;
            Response[7] = (digitalRead(8) == HIGH) ? 1 : 0;
            Response[8] = (digitalRead(9) == HIGH) ? 1 : 0;
            Response[9] = (digitalRead(10) == HIGH) ? 1 : 0;
            Response[10] = (digitalRead(11) == HIGH) ? 1 : 0;
            Response[11] = (digitalRead(12) == HIGH) ? 1 : 0;
            Response[12] = (digitalRead(A2) == HIGH) ? 1 : 0;
            Response[13] = (digitalRead(A3) == HIGH) ? 1 : 0;
            break;
        case 2: // Mode: Set Device State
            Response[0] = (digitalRead(Pin) == HIGH) ? 1 : 0;
            break;
        default:
            break;
    }

  // Wire back response
  Wire.write(Response, 14);
}

