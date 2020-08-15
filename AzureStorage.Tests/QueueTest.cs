using AzureStorage.Queue;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AzureStorage.Tests
{
    [TestClass]
    public sealed class QueueTest
    {
        QueueManager manager;
        Queue.Queue queue;
        const string queueName = "queuetest1";
        const string messageText = "message text 1";

        [TestInitialize]
        public void Initialize()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<QueueTest>()
                .Build();
            
            string connectionString = config["Queue:ConnectionString"];

            manager = new QueueManager(connectionString);
            queue = manager.CreateQueueAsync(queueName).Result;

            Assert.IsNotNull(queue);
            Assert.IsTrue(manager.ExistsQueueAsync(queueName).Result);
            Assert.AreEqual(queue.Name, queueName);
        }

        [TestCleanup]
        public void Cleanup()
        {
            //bool deleted = manager.DeleteQueueAsync(queueName).Result;

            //Assert.IsTrue(deleted);
            //Assert.IsFalse(manager.ExistsQueueAsync(queueName).Result);
        }

        [TestMethod]
        public void AddMessage()
        {
            int count = queue.MessageCount;
            var message = queue.SendMessageAsync(messageText).Result;

            Assert.AreEqual(queue.MessageCount, count + 1);
            Assert.IsNotNull(message.Id);
            Assert.AreEqual(message.Text, messageText);
        }

        [TestMethod]
        public void PeekMessage()
        {
            AddMessage();

            int count = queue.MessageCount;
            var message = queue.PeekMessageAsync().Result;

            Assert.AreEqual(queue.MessageCount, count);
            Assert.IsNotNull(message.Id);
            Assert.AreEqual(message.Text, messageText);
        }

        [TestMethod]
        public void ReceiveMessage()
        {
            AddMessage();

            int count = queue.MessageCount;
            var message = queue.ReceiveMessageAsync().Result;

            Assert.AreEqual(queue.MessageCount, count);
            Assert.IsNotNull(message.Id);
            Assert.AreEqual(message.Text, messageText);
        }

        [TestMethod]
        public void UpdateMessage()
        {
            const string newText = "New Text";

            int count = queue.MessageCount;
            var message = queue.SendMessageAsync(messageText).Result;
            
            message.Text = newText;
            var updated = queue.UpdateMessageAsync(message).Result;

            Assert.AreEqual(queue.MessageCount, count + 1);
            Assert.AreEqual(updated.Id, message.Id);
            Assert.AreEqual(updated.Text, newText);
        }

        [TestMethod]
        public void DeleteMessage()
        {
            int count = queue.MessageCount;
            var message = queue.SendMessageAsync(messageText).Result;
            bool deleted = queue.DeleteMessageAsync(message).Result;

            Assert.AreEqual(queue.MessageCount, count);
            Assert.IsTrue(deleted);
        }
    }
}
