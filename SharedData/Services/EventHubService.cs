using Azure.Storage.Queues;
using System.Text.Json;

namespace SharedData.Services
{
    public class QueueService
    {
        private readonly QueueClient _queueClient;

        public QueueService(string connectionString, string queueName)
        {
            _queueClient = new QueueClient(connectionString, queueName);
            _queueClient.CreateIfNotExists();
        }

        public async Task SendMessageAsync<T>(T messageData)
        {
            var messageBody = JsonSerializer.Serialize(messageData);
            await _queueClient.SendMessageAsync(messageBody);
        }
    }
}