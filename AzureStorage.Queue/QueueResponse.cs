using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AzureStorage.Queue
{
    public class QueueResponse
    {
        public QueueResponse() : this(HttpStatusCode.Unused, string.Empty)
        {
        }

        public QueueResponse(HttpStatusCode status) : this(status, string.Empty)
        {
        }

        public QueueResponse(HttpStatusCode status, string message)
        {
            Status = status;
            Message = message;
        }

        public HttpStatusCode Status { get; internal set; }

        public string Message { get; internal set; }
    }
}
