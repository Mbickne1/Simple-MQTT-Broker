using MQTTnet.Server;
using MQTTnet;
using System.Text;
using static System.Console;

namespace MQTTBroker {

    public class MQTTBroker {

        private static IMqttServer mqttServer = default!;

        static async Task Main(string[] args) {
            // Create the options for MQTT Broker
            MqttServerOptionsBuilder builder =
                new MqttServerOptionsBuilder()
                // Set endpoint to localhost
                .WithDefaultEndpointPort(5004)
                // WithDefaultEndpointPort(5004) -- Changes Port Endpoint
                // Add interceptor for logging incoming messages
                .WithApplicationMessageInterceptor(OnNewMessage);

            // Create the server
            mqttServer = new MqttFactory().CreateMqttServer();
            await mqttServer.StartAsync(builder.Build());
            Console.WriteLine("### SERVER STARTED SUCCESSFULLY ###");
            // Keep application running until user presses a key
            ReadLine();
        }

        static void OnNewMessage(MqttApplicationMessageInterceptorContext context) {
            // Ensure the message is not being sent by the broker
            if(context.ClientId != null) {
                var payload = context.ApplicationMessage?.Payload == null ? null : Encoding.UTF8.GetString(context.ApplicationMessage?.Payload);
                
                // await MessageToSubcriber();
                
                WriteLine(
                    " TimeStamp: {0} -- Message: ClientId = {1}, Topic = {2}, Payload = {3}, QoS = {4}, Retain-Flag = {5}",
                    DateTime.Now,
                    context.ClientId,
                    context.ApplicationMessage?.Topic,
                    payload,
                    context.ApplicationMessage?.QualityOfServiceLevel,
                    context.ApplicationMessage?.Retain
                );
            }
        }
    }
}
