using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRMvcChat.Web.Domain
{
    public class ProcessCount
    {
        public string QueueName { get; set; }
        public int Length { get; set; }

        public ProcessCount(string queueName, int count)
        {
            QueueName = queueName;
            Length = count;
        }

        public ProcessCount()
        { }
    }
}