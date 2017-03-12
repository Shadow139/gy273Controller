#include <Wire.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_BNO055.h>
#include <utility/imumaths.h>

/* This driver reads raw data from the BNO055

   Connections
   ===========
   Connect SCL to analog 5
   Connect SDA to analog 4
   Connect VDD to 3.3V DC
   Connect GROUND to common ground

   History
   =======
   2015/MAR/03  - First release (KTOWN)
*/

/* Set the delay between fresh samples */
#define BNO055_SAMPLERATE_DELAY_MS (100)

Adafruit_BNO055 bno = Adafruit_BNO055();

const int JButtonPin = 2;
const int XPin = 0;
const int YPin = 1;

const int RPin = 12;
const int GPin = 11;
const int BPin = 10;
/**************************************************************************/
/*
    Arduino setup function (automatically called at startup)
*/
/**************************************************************************/
void setup(void)
{

  //Joystick
  pinMode(JButtonPin, INPUT);
  digitalWrite(JButtonPin,HIGH);

  //RGB  
  //pinMode(RPin, OUTPUT);
  //pinMode(GPin, OUTPUT);
  //pinMode(BPin, OUTPUT);
  
  Serial.begin(38400);
  //Serial.println("Orientation Sensor Raw Data Test"); Serial.println("");

  /* Initialise the sensor */
  if(!bno.begin())
  {
    /* There was a problem detecting the BNO055 ... check your connections */
    Serial.print("Ooops, no BNO055 detected ... Check your wiring or I2C ADDR!");
    while(1);
  }

  delay(1000);

  /* Display the current temperature */
  int8_t temp = bno.getTemp();
  //Serial.print("Current Temperature: ");
  //Serial.print(temp);
  //Serial.println(" C");
  //Serial.println("");

  bno.setExtCrystalUse(true);

  //Serial.println("Calibration status values: 0=uncalibrated, 3=fully calibrated");
}

/**************************************************************************/
/*
    Arduino loop function, called once 'setup' is complete (your own code
    should go here)
*/
/**************************************************************************/
void loop(void)
{
  // Possible vector values can be:
  // - VECTOR_ACCELEROMETER - m/s^2
  // - VECTOR_MAGNETOMETER  - uT
  // - VECTOR_GYROSCOPE     - rad/s
  // - VECTOR_EULER         - degrees
  // - VECTOR_LINEARACCEL   - m/s^2
  // - VECTOR_GRAVITY       - m/s^2
  imu::Vector<3> euler = bno.getVector(Adafruit_BNO055::VECTOR_EULER);

  sensors_event_t event;
  bno.getEvent(&event);

  //digitalWrite(RPin,LOW);
  //digitalWrite(GPin,LOW);
  //digitalWrite(BPin,LOW);


  Serial.print("J ");
  Serial.print(digitalRead(JButtonPin)); 
  Serial.print(" ");
  Serial.print(analogRead(XPin)); 
  Serial.print(" ");
  Serial.println(analogRead(YPin));
  //Serial.print("|");
  
  
  // Quaternion data
  imu::Quaternion quat = bno.getQuat();
  Serial.print("Q ");
  Serial.print(quat.x(), 4);
  Serial.print(" ");
  Serial.print(quat.y(), 4);
  Serial.print(" ");
  Serial.print(quat.z(), 4);
  Serial.print(" ");
  Serial.println(quat.w(), 4);  
  //Serial.print("|");
  
  Serial.print("A ");
  Serial.print((float)event.acceleration.x);
  Serial.print(" ");
  Serial.print((float)event.acceleration.y);
  Serial.print(" ");
  Serial.println((float)event.acceleration.z);
  //Serial.print("|");

  imu::Vector<3> lineacc = bno.getVector(Adafruit_BNO055::VECTOR_LINEARACCEL);
  Serial.print("L ");
  Serial.print(lineacc.x());
  Serial.print(" ");
  Serial.print(lineacc.y());
  Serial.print(" ");
  Serial.println(lineacc.z());
  //Serial.print("|");

  /* Display calibration status for each sensor. */
  uint8_t system, gyro, accel, mag = 0;
  bno.getCalibration(&system, &gyro, &accel, &mag);
  Serial.print("C ");
  Serial.print(system, DEC);
  Serial.print(" ");
  Serial.print(gyro, DEC);
  Serial.print(" ");
  Serial.print(accel, DEC);
  Serial.print(" ");
  Serial.println(mag, DEC);
  
  delay(BNO055_SAMPLERATE_DELAY_MS);
}
