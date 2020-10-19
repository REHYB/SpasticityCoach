/*
   Copyright (c) 2016 Intel Corporation.  All rights reserved.
   See the bottom of this file for the license terms.
*/

/*
   This sketch example demonstrates how the BMI160 on the
   Intel(R) Curie(TM) module can be used to read accelerometer data
*/
// More information at: https://www.arduino.cc/en/Reference/CurieBLE

#include "CurieIMU.h"
#include <Servo.h>
#include <CurieBLE.h>


BLEService motorService("78323a84-b5a4-4231-898f-6f422acc6235"); //custom 120-bit UUID custom service
//imu properties


// twelve servo objects can be created on most boards

float timeMillis = 0;
float timeMillisMotionReset = 0;
float timeMillisGyroReset = 0;
float ax, ay, az, atot, gx, gy, gz, gtot, gx_bias, gy_bias, gz_bias, gtot_filt;
int motionDetect = 0;
int switchPosition = 0; // fingers extended
int autoButton = 0;
int autoButtonOld = 0;
int extendButton = 0;
int extendButtonOld = 0;
int extendButtonChange = 0;
int delayMode = 0;
int timeMillisDelayMode = 0;
int extendMotor = 55; //50 = fully squeeze bottle; 80 = partially squeeze bottle
int retractMotor = 150; //150 = fully extend fingers; 120 = partially extend fingers
int manualExtend = 0;
int manualContract = 1;
int manualRelax = -1;

void setup() {
  timeMillis = millis();
  CurieIMU.begin();
  
  // Set the accelerometer range to 2G
  CurieIMU.setAccelerometerRange(2);
  // Set the accelerometer range to 250 degrees/second
  CurieIMU.setGyroRange(250);

  BLE.begin();
  BLE.setLocalName("FlexSleeve");
  BLE.setAdvertisedService(motorService);  // add the service UUID
  motorService.addCharacteristic(motorReading);
  motorService.addCharacteristic(motorExtendChar);
  motorService.addCharacteristic(motorContractChar);
  motorService.addCharacteristic(gxChar);
  BLE.addService(motorService);   // Add the BLE Battery service

  motorReading.setValue(manualRelax);
  motorExtendChar.setValue(extendMotor);
  motorContractChar.setValue(2);//retractMotor);
  gxChar.setValue(2);//retractMotor);
  BLE.advertise();

  
  extendMotor = motorExtendChar.value();
  retractMotor = motorContractChar.value();

  //read buttons
  if (digitalRead(2) == HIGH)
  {
    autoButton = 1;
  }
  else
  {
    autoButton = 0;
  }
  if (digitalRead(6) == HIGH)
  {
    extendButton = 1;
  }
  else
  {
    extendButton = 0;
  }

  // if auto button is pressed down we are in manual mode, start manual mode with motors relaxed
  if (autoButton == 1)
  { myservoflex.write(retractMotor);
    myservoextend.write(retractMotor);
  }

  // if auto button is pressed up we are in auto mode, start auto mode with relaxed position
  else if (autoButton == 0)
  {
    myservoflex.write(retractMotor);
    myservoextend.write(retractMotor);
    delayMode = 1;
    timeMillisDelayMode = millis();
  }
  //Serial.begin(9600);
}

void loop() {
  //on loop reset the motor values
  extendMotor = motorExtendChar.value();
  //retractMotor = motorContractChar.value();

  // read accelerometer measurements from device, scaled to the configured range
  CurieIMU.readAccelerometerScaled(ax, ay, az);
  CurieIMU.readGyroScaled(gx, gy, gz);
  motorContractChar.setValue(gx);

  //calculate absolute value of gyro
  gtot = sqrt(sq(gx - gx_bias) + sq(gy - gy_bias) + sq(gz - gz_bias));
  //filter gtot so quick noise is not detected as motion
  gtot_filt = gtot * 0.2 + gtot_filt * 0.8;

  //Serial.println(motorExtendChar.value());
  //Serial.println(motorContractChar.value());
  //Serial.print(motorReading.value());
  if (motorReading.value()== 1)
    {myservoflex.write(retractMotor);
    myservoextend.write(extendMotor);}
  else if (motorReading.value()== 0)
    {myservoflex.write(extendMotor);
    myservoextend.write(retractMotor);}
  else
    {myservoflex.write(extendMotor);
    myservoextend.write(extendMotor);}
  
}//end of void loop

/*
   Copyright (c) 2016 Intel Corporation.  All rights reserved.
   This library is free software; you can redistribute it and/or
   modify it under the terms of the GNU Lesser General Public
   License as published by the Free Software Foundation; either
   version 2.1 of the License, or (at your option) any later version.
   This library is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
   Lesser General Public License for more details.
   You should have received a copy of the GNU Lesser General Public
   License along with this library; if not, write to the Free Software
   Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/
