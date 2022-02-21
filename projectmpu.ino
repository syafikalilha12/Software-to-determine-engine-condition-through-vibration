#include <Wire.h>
#include<math.h>
#define M_PI 3.141592653589793238462643
const int MPU_addr=0x68;  // I2C address of the MPU-6050
int16_t AcX,AcY,AcZ,Tmp,GyX,GyY,GyZ;
int16_t X,Y,Z;
char dl;
void setup(){
  Wire.begin();
  Wire.beginTransmission(MPU_addr);
  Wire.write(0x6B);  // PWR_MGMT_1 register
  Wire.write(0);     // set to zero (wakes up the MPU-6050)
  Wire.endTransmission(true);
  Serial.begin(9600);
 
}
void loop(){
        dl=Serial.read();
        Wire.beginTransmission(MPU_addr);
        Wire.write(0x3B);  // starting with register 0x3B (ACCEL_XOUT_H)
        Wire.endTransmission(false);
        Wire.requestFrom(MPU_addr,14,true);  // request a total of 14 registers
        AcX=Wire.read()<<8|Wire.read();  // 0x3B (ACCEL_XOUT_H) & 0x3C (ACCEL_XOUT_L)  
        AcY=Wire.read()<<8|Wire.read();  // 0x3D (ACCEL_YOUT_H) & 0x3E (ACCEL_YOUT_L)
        AcZ=Wire.read()<<8|Wire.read();  // 0x3F (ACCEL_ZOUT_H) & 0x40 (ACCEL_ZOUT_L)
        Tmp=Wire.read()<<8|Wire.read();  // 0x41 (TEMP_OUT_H) & 0x42 (TEMP_OUT_L)
        GyX=Wire.read()<<8|Wire.read();  // 0x43 (GYRO_XOUT_H) & 0x44 (GYRO_XOUT_L)
        GyY=Wire.read()<<8|Wire.read();  // 0x45 (GYRO_YOUT_H) & 0x46 (GYRO_YOUT_L)
        GyZ=Wire.read()<<8|Wire.read();  // 0x47 (GYRO_ZOUT_H) & 0x48 (GYRO_ZOUT_L)
    
    
    //  Serial.print(" | GyX = "); Serial.print(GyX);
    //  Serial.print(" | GyY = "); Serial.print(GyY);
    //  Serial.print(" | GyZ = "); Serial.println(GyZ);
     
        X=180*atan(AcX/sqrt(AcY*AcY+AcZ*AcZ))/M_PI;
        Y=180*atan(AcY/sqrt(AcX*AcX+AcZ*AcZ))/M_PI;
        Z=180*atan(AcZ/sqrt(AcX*AcX+AcZ*AcZ))/M_PI;

        Serial.print(X);
        Serial.print(",");
        Serial.print(Y);
        Serial.print(",");
        Serial.print(AcX);//acc x data 2
        Serial.print(",");
        Serial.print(AcY);//acc Y data 3
        Serial.print(",");
        Serial.print(AcZ);//acc Z data 4
        Serial.print(",");
        Serial.print(GyX);//gyro X data 5
        Serial.print(",");
        Serial.print(GyY);//gyro Y data 6
        Serial.print(",");
        Serial.print(GyZ);//gyro Z data 7
        Serial.print(",");
        if(AcX>=-4000&&AcX<=1500)
        {
          Serial.println("GOOD");
        }
        else
        {
          Serial.println("BAD");
        }
        delay(500);
      }
      
   
