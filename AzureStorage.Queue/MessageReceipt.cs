using System;

namespace AzureStorage.Queue
{
    public class MessageReceipt
    {
        public string Id { get; internal set; }

        public string Text { get; set; }

        public string PopReceipt { get; internal set; }

        public DateTimeOffset? InsertionTime { get; internal set; }

        public DateTimeOffset? ExpirationTime { get; internal set; }
    }
}
