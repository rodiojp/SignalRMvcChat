using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRMvcChat.Web.Domain;

namespace SignalRMvcChat.Web.Models
{
    public class ProcessCountVm:ProcessCount
    {
        public DateTime Date { get; private set; }

        public ProcessCountVm(ProcessCount processCount)
        {
            Date = DateTime.Now;
            QueueName = processCount.QueueName;
            Length = processCount.Length;
        }
        public static IEnumerable<ProcessCountVm> BuildViewModels(IEnumerable<ProcessCount> processCounts)
        {
            return processCounts.Select(x => new ProcessCountVm(x));
        }
    }
}