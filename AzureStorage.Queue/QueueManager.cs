using Azure.Storage.Queues;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorage.Queue
{
    public class QueueManager
    {
        public QueueManager(string connectionString)
        {
            _connectionString = connectionString;
            _queues = new Dictionary<string, Queue>();
        }

        public async Task<Queue> CreateQueueAsync(string name)
        {
            string key = name.ToLowerInvariant();

            if (_queues.ContainsKey(key))
            {
                return _queues[key];
            }

            Queue queue = await Queue.CreateAsync(_connectionString, key);
            _queues.Add(key, queue);

            return queue;
        }

        public async Task<Queue> GetQueueAsync(string name)
        {
            string key = name.ToLowerInvariant();

            return await ExistsQueueAsync(name) ? _queues[key] : null;
        }

        public async IAsyncEnumerable<Queue> GetAllQueuesAsync()
        {
            foreach (var q in _queues.Values)
            {
                yield return await GetQueueAsync(q.Name);
            }
        }

        public async Task<bool> DeleteQueueAsync(string name)
        {
            string key = name.ToLowerInvariant();
            bool deleted = false;

            if (_queues.ContainsKey(key))
            {
                deleted = await _queues[key].DeleteAsync();
                _queues.Remove(key);
            }

            return deleted;
        }

        public async Task<bool> ClearQueueAsync(string name)
        {
            string key = name.ToLowerInvariant();

            return _queues.ContainsKey(key) && await _queues[key].ClearAsync();
        }

        public async Task<bool> ExistsQueueAsync(string name)
        {
            string key = name.ToLowerInvariant();

            return _queues.ContainsKey(key) && await _queues[key].ExistsAsync();
        }

        private readonly string _connectionString;
        private readonly IDictionary<string, Queue> _queues;
    }
}
