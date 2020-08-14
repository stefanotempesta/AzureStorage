using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AzureStorage.Web.Models
{
    public class QueueMessage
    {
        public string Id { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
    }
}
