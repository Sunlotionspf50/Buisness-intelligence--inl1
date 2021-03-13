void initDevice(){
  Esp32MQTTClient_Init((uint8_t *)conn, true);
  Esp32MQTTClient_SetSendConfirmationCallback(SendCallback);

}

void SendCallback(IOTHUB_CLIENT_CONFIRMATION_RESULT result) {
  if(result == IOTHUB_CLIENT_CONFIRMATION_OK) {
    Serial.println("Message confirmed.");
    messagePending = false; //Väntar på callback för att nollställa messagePending
  }
}

void sendMessage(char *payload){ //payload är meddelandet som ska skickas.
  messagePending = true;
  EVENT_INSTANCE *message = Esp32MQTTClient_Event_Generate(payload, MESSAGE);

  Esp32MQTTClient_Event_AddProp(message, "name", "Jonathan Koitsalu");
  Esp32MQTTClient_Event_AddProp(message, "school", "Nackademin");
  Esp32MQTTClient_SendEventInstance(message);

}
