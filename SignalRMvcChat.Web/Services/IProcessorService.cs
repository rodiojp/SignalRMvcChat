using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRMvcChat.Web.Domain;

namespace SignalRMvcChat.Web.Services
{
    public interface IProcessorService
    {
        IEnumerable<ProcessCount> GetCounts();
        StuckInProcessingDto FindStuckProcessingQueues(string table, string column, int maxRetries);
        StuckInProcessingDto FindStuckProcessingQueues();
        StuckInProcessingDto FindStuckProcessingGPSContext();

    }
}