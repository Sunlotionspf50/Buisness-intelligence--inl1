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
char *conn = "HostName=iot20-jk-new.azure-devices.net;DeviceId=Esp32;SharedAccessKey=yUFEj3OhKXllLBZgV/T5PFeQ1lhKUJ0I4PBs7qx7O5s=";

bool messagePending = false;

float a = 0;
float b = 1;

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

      float fibonacci = a + b;
      a = b;
      b = fibonacci;

      float LottoVinstChansProcent = random(0, 100);



      char payload[256];

      DynamicJsonDocument doc(1024);
      doc["Fibonacci"] = fibonacci;
      doc["LottoVinstChansProcent"] = LottoVinstChansProcent;
      doc["EpochTime"] = epochTime;
      serializeJson(doc, payload);

      sendMessage(payload);
    }
  }
  Esp32MQTTClient_Check(); //kontrollerar om den har fått nya meddelanden till sig själv.
  delay(20000);
}
