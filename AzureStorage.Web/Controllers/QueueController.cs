using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AzureStorage.Web.Controllers
{
    public class QueueController : Controller
    {
        private readonly string _connectionString;
        private Queue.QueueService _service;

        public QueueController(IConfiguration configuration)
        {
            // Queue:ConnectionString is configured in User Secrets
            _connectionString = configuration["Queue:ConnectionString"];
        }

        public IActionResult Index(Models.Queue queue)
        {
            return View(queue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQueue(Models.Queue model)
        {
            if (ModelState.IsValid)
            {
                _service = new Queue.QueueService(_connectionString);
                await _service.CreateQueueAsync(model.Name);
            }

            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClearQueue(string name)
        {
            _service = new Queue.QueueService(_connectionString);
            await _service.ClearQueueAsync(name);

            return View("Index", new Models.Queue(name));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteQueue(string name)
        {
            _service = new Queue.QueueService(_connectionString);
            await _service.DeleteQueueAsync(name);

            return View("Index", new Models.Queue());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMessage(string name, string messageText)
        {
            _service = new Queue.QueueService(_connectionString);
            await _service.AddMessageAsync(name, messageText);

            var messages = from msg in await _service.GetMessagesAsync(name)
                           select new Models.QueueMessage
                           {
                                Id = msg.Id,
                                Text = msg.Text,
                                CreatedOn = msg.InsertionTime.Value
                           };

            return View("Index", new Models.Queue(name) { Messages = messages.ToList() });
        }
    }
}
