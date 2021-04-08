using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRMvcChat.Web.Domain;
using SignalRMvcChat.Web.Models;
using SignalRMvcChat.Web.Services;

namespace SignalRMvcChat.Web.Hubs
{
    /// <summary>
    /// Implements a process to get queue data and broadcast the client
    /// </summary>
    /// <remarks>
    /// How it works: Calling Start enables the timer and does a thread save call to the 
    /// every half second to the <see cref="ProcessorService"/> to get the counts of 
    /// intersted queues
    /// </remarks>
    public class QueuesTicker 
    {
        //half second interval
        private const int UPDATE_INTERVAL = 500; //ms
        //Lazy implementation to improve performance and not wasteful -- remember want to get everything doen in .5 seconds
        private readonly static Lazy<QueuesTicker> instance = new Lazy<QueuesTicker>(() => new QueuesTicker(GlobalHost.ConnectionManager.GetHubContext<ProcessingQueuesHub>().Clients));

        private static readonly ProcessorService QueuesCounter = new ProcessorService();

        //
        private Timer timer;
        private readonly object updateQueueLock = new object();
        private bool updatingCounts;

        public QueuesTicker()
        {
            timer = new Timer(UpdateQueue, null, Timeout.Infinite, Timeout.Infinite);
        }
        private QueuesTicker(IHubConnectionContext<dynamic> clients) :this()
        {
            Clients = clients;
        }
        private IHubConnectionContext<dynamic> Clients
        {
            get;
            set;
        }
        public bool Start()
        {
            timer.Change(0, UPDATE_INTERVAL);
            return true;
        }
        public void Stop()
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
        }
        private void UpdateQueue(object state)
        {
            //if still in process of updating, wait till next time
            if (updatingCounts)
            {
                return;
            }
            lock (updateQueueLock)
            {
                if (updatingCounts) return;
                updatingCounts = true;
                //get the data and broadcast
                try
                {
                    BroadcastQueuesLevel(QueuesCounter.GetCounts());
                }
                catch (Exception ex)
                {
                    //just swallow the exception
                }
                finally
                {
                    updatingCounts = false;
                }
            }
        }
        /// <summary>
        /// get the lazy value
        /// </summary>
        public static QueuesTicker Instance
        {
            get { return instance.Value; }
        }
        private void BroadcastQueuesLevel(IEnumerable<ProcessCount> processCounts)
        {
            var processCountVms = ProcessCountVm.BuildViewModels(processCounts);
            Clients.All.updateQueuesCounts(processCountVms);
        }
        private IEnumerable<ProcessCountVm> FakeProcessNumbers()
        {
            var random = new Random();
            return new ProcessCountVm[]
            {
                new ProcessCountVm(new ProcessCount("balMtrQueue",random.Next(0,1000))),
                new ProcessCountVm(new ProcessCount("balStnQueue",random.Next(0,1000))),
                new ProcessCountVm(new ProcessCount("balStnWaitQueue",random.Next(0,1000))),
                new ProcessCountVm(new ProcessCount("balWorkSet",random.Next(0,1000))),
                new ProcessCountVm(new ProcessCount("rdm_ReadingRawHy",random.Next(0,1000))),
                new ProcessCountVm(new ProcessCount("rdm_ReadingRawDy",random.Next(0,1000))),
            };
        }
    }
}