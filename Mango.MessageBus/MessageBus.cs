using System;
using System.Net;
using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace Mango.MessageBus
{
	public class MessageBus : IMessageBus
	{
        private string _connectionString = "Endpoint = sb://mbadadymangoweb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=j4V/8eNhhqn3vQD2Lg0i99aOS6GFN0TvZ+ASbIAIQBQ=";


        public async Task PublishMessage(object message, string topic_queue_name, string connectionString)
        {
            await using var client = new ServiceBusClient(_connectionString);

            ServiceBusSender sender = client.CreateSender(topic_queue_name);

            var jsonMessage = JsonConvert.SerializeObject(message);

            ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };

            await sender.SendMessageAsync(finalMessage);

            await client.DisposeAsync();
            
        }
    }
}

