using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRMvcChat.Web.Domain;

namespace SignalRMvcChat.Web.Services
{
    /// <summary>
    /// Check Status of Process
    /// </summary>
    /// <remarks>
    /// Note: There was no way that I could do the dynamic SQL or UNION statements with
    /// multiple trips to the database
    /// </remarks>
    public class ProcessorService : IProcessorService
    {
        public StuckInProcessingDto FindStuckProcessingGPSContext()
        {
            throw new NotImplementedException();
        }

        public StuckInProcessingDto FindStuckProcessingQueues(string table, string column, int maxRetries)
        {
            throw new NotImplementedException();
        }

        public StuckInProcessingDto FindStuckProcessingQueues()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProcessCount> GetCounts()
        {
            //const string sql = @"SELECT [QueueName] = 'rdm_ReadingRawDy', [Length] = COUNT(*) FROM [DATABASE_NAME].[dbo].[rdm_ReadingRawDy]
            //                     UNION
            //                     SELECT [QueueName] = 'operationReuestQueue', [Length] = COUNT(*) FROM [DATABASE_NAME].[dbo].[operationReuestQueue]";
            //using (var db = new DATABASE_NAMEEntities())
            //{
            //    db.Database.CommandTimeout = 5 * 60;
            //    var result = db.SqlQuery<ProcessCount>(sql).ToList();
            //    return result;
            //}
            Random random = new Random();
            return new ProcessCount[]
            {
                new ProcessCount("balMtrQueue",random.Next(0,1000)),
                new ProcessCount("balStnQueue",random.Next(0,1000)),
                new ProcessCount("balStnWaitQueue",random.Next(0,1000)),
                new ProcessCount("balWorkSet",random.Next(0,1000)),
                new ProcessCount("rdm_ReadingRawHy",random.Next(0,1000)),
                new ProcessCount("rdm_ReadingRawDy",random.Next(0,1000))
            };
        }
    }
}