#include <WiFi.h>
#include "time.h"
#include "Esp32MQTTClient.h"
#include "AzureIoTHub.h"
#include <DHT.h>
#include <ArduinoJson.h>

#define INTERVAL 1000 * 5

#define DHT_TYPE DHT11
#define DHT_PIN 14

//instanser initieras:

DHT dht(DHT_PIN, DHT_TYPE);

char *ssid = "MORMOR";
char *pass = "skansbergsvagen";
char *conn = "HostName=iot20-jk-new.azure-devices.net;DeviceId=AC:67:B2:08:FE:A8;SharedAccessKey=zLfRaN5kE8h7Q1XWDqmy9073kXjaD90dUTrqx1wFnkc=";

bool messagePending = false;

float latestTemperature = 0;
float latestHumidity = 0;

int interval = 2000;
unsigned long prevMillis = 0;
time_t epochTime;

void setup() {
  initSerial();
  initWifi();
  initDHT();
  initDevice();
  initEpochTime();
  

  delay (2000);

}

void loop() {

  //Bygga ihop meddelandet om meddelande inte skickas just nu.
  

  unsigned long currentMillis = millis();
  float temperature = dht.readTemperature();
  float humidity = dht.readHumidity();
  epochTime = time(NULL);                         //Ger epochTime med time-funktionen som initialiserades med initEpochTime.
      
  if (!messagePending) {
    if ((currentMillis - prevMillis) >= INTERVAL) {
      prevMillis = currentMillis;
      
      //skapa json-meddelandet.
      
      if (!std::isnan(temperature) && !std::isnan(humidity) && (abs(temperature - latestTemperature) > 1 || abs(humidity - latestHumidity) > 1)) { //kolla så att datat inte är nan samt att data hunnit ändra sig med en grad eller humidity.

        latestTemperature = temperature;
        latestHumidity = humidity;

        char payload[256];

        DynamicJsonDocument doc(1024);
        doc["epochtime"] = epochTime;
        doc["temperature"] = temperature;
        doc["humidity"] = humidity;
        serializeJson(doc, payload);

        sendMessage(payload);
      }
    }
  }

  Esp32MQTTClient_Check(); //kontrollerar om den har fått nya meddelanden till sig själv.
  delay(10);
}
