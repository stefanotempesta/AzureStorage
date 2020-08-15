using Azure.Storage.Queues;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace AzureStorage.Queue
{
    // Stateless service implementation
    public class QueueService
    {
        public QueueService(string connectionString)
        {
            _service = new QueueServiceClient(connectionString);
        }

        public async Task CreateQueueAsync(string name)
        {
            await _service.CreateQueueAsync(name.ToLowerInvariant());
        }

        public async Task DeleteQueueAsync(string name)
        {
            await _service.DeleteQueueAsync(name.ToLowerInvariant());
        }

        public async Task ClearQueueAsync(string name)
        {
            await _service.GetQueueClient(name.ToLowerInvariant()).ClearMessagesAsync();
        }

        public async Task<bool> ExistsQueueAsync(string name)
        {
            return await _service.GetQueueClient(name.ToLowerInvariant()).ExistsAsync();
        }

        public async Task<MessageReceipt> AddMessageAsync(string queueName, string messageText)
        {
            var receipt = await _service.GetQueueClient(queueName.ToLowerInvariant()).SendMessageAsync(messageText);

            return new MessageReceipt
            {
                Id = receipt.Value.MessageId,
                Text = messageText,
                PopReceipt = receipt.Value.PopReceipt,
                InsertionTime = receipt.Value.InsertionTime,
                ExpirationTime = receipt.Value.ExpirationTime
            };
        }

        public async Task<IEnumerable<MessageReceipt>> GetMessagesAsync(string queueName)
        {
            var queue = _service.GetQueueClient(queueName.ToLowerInvariant());
            var messages = await queue.PeekMessagesAsync(queue.MaxPeekableMessages);

            return from msg in messages.Value
                   select new MessageReceipt
                   {
                       Id = msg.MessageId,
                       Text = msg.MessageText,
                       InsertionTime = msg.InsertedOn,
                       ExpirationTime = msg.ExpiresOn
                   };
        }

        private readonly QueueServiceClient _service;
    }
}
