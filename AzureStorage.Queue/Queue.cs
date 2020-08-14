using Azure;
using Azure.Storage.Queues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorage.Queue
{
    public class Queue
    {
        internal static async Task<Queue> CreateAsync(string connectionString, string name)
        {
            Queue instance = new Queue();
            await instance.InitializeAsync(connectionString, name);

            return instance;
        }

        private Queue()
        {
        }

        private async Task InitializeAsync(string connectionString, string name)
        {
            _queue = new QueueClient(connectionString, name);

            await _queue.CreateIfNotExistsAsync();

            Name = name;
        }

        public string Name { get; private set; }

        public int MessageCount => _queue.GetProperties().Value.ApproximateMessagesCount;

        public async Task<bool> DeleteAsync()
        {
            var response = await _queue.DeleteIfExistsAsync();

            return response.Value;
        }

        public async Task<bool> ClearAsync()
        {
            await _queue.ClearMessagesAsync();

            return _queue.MaxPeekableMessages == 0;
        }

        public async Task<bool> ExistsAsync()
        {
            var response = await _queue.ExistsAsync();

            return response.Value;
        }

        public async Task<MessageReceipt> SendMessageAsync(string message)
        {
            var receipt = await _queue.SendMessageAsync(message);

            return new MessageReceipt
            {
                Id = receipt.Value.MessageId,
                Text = message,
                PopReceipt = receipt.Value.PopReceipt,
                InsertionTime = receipt.Value.InsertionTime,
                ExpirationTime = receipt.Value.ExpirationTime
            };
        }

        public async Task<MessageReceipt> PeekMessageAsync()
        {
            var messages = await _queue.PeekMessagesAsync();
            var receipt = messages.Value.FirstOrDefault();

            if (receipt == null) return null;
            
            return new MessageReceipt
            {
                Id = receipt.MessageId,
                Text = receipt.MessageText,
                InsertionTime = receipt.InsertedOn,
                ExpirationTime = receipt.ExpiresOn
            };
        }

        public async Task<IEnumerable<MessageReceipt>> PeekMessagesAsync(int count)
        {
            var messages = await _queue.PeekMessagesAsync(count);
            var receipts = messages.Value
                .Select(m => new MessageReceipt
                {
                    Id = m.MessageId,
                    Text = m.MessageText,
                    InsertionTime = m.InsertedOn,
                    ExpirationTime = m.ExpiresOn
                });

            return receipts;
        }

        public async Task<MessageReceipt> ReceiveMessageAsync()
        {
            var messages = await _queue.ReceiveMessagesAsync();
            var receipt = messages.Value.FirstOrDefault();

            if (receipt == null) return null;

            return new MessageReceipt
            {
                Id = receipt.MessageId,
                Text = receipt.MessageText,
                PopReceipt = receipt.PopReceipt,
                InsertionTime = receipt.InsertedOn,
                ExpirationTime = receipt.ExpiresOn
            };
        }

        public async Task<IEnumerable<MessageReceipt>> ReceiveMessagesAsync(int count)
        {
            var messages = await _queue.ReceiveMessagesAsync(count);
            var receipts = messages.Value
                .Select(m => new MessageReceipt
                {
                    Id = m.MessageId,
                    Text = m.MessageText,
                    PopReceipt = m.PopReceipt,
                    InsertionTime = m.InsertedOn,
                    ExpirationTime = m.ExpiresOn
                });

            return receipts;
        }

        public async Task<MessageReceipt> UpdateMessageAsync(MessageReceipt message)
        {
            var receipt = await _queue.UpdateMessageAsync(message.Id, message.PopReceipt, message.Text);

            return new MessageReceipt
            {
                Id = message.Id,
                Text = message.Text,
                PopReceipt = receipt.Value.PopReceipt,
                InsertionTime = message.InsertionTime,
                ExpirationTime = message.ExpirationTime
            };
        }

        public async Task<bool> DeleteMessageAsync(MessageReceipt message)
        {
            await _queue.DeleteMessageAsync(message.Id, message.PopReceipt);

            return true;
        }

        private QueueClient _queue;
    }
}
