using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalRMvcChat.Web.Hubs
{
    public class ProcessingQueuesHub : Hub
    {
        public static readonly HashSet<string> Connected = new HashSet<string>();

        private readonly QueuesTicker queuesTicker;

        public ProcessingQueuesHub() : this(QueuesTicker.Instance)
        { }

        public ProcessingQueuesHub(QueuesTicker queuesTicker)
        {
            this.queuesTicker = queuesTicker;
        }

        public bool Start()
        {
            return queuesTicker.Start();
        }

        public override Task OnConnected()
        {
            Trace.TraceInformation("MapHub started. ID: {0}", Context.ConnectionId);
            lock (Connected)
            {
                Connected.Add(Context.ConnectionId);
            }
            return base.OnConnected();
        }
        public void AfterConnected()
        {
            Clients.Caller.hello("message");
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            lock (Connected)
            {
                Connected.Remove(Context.ConnectionId);
                if (Connected.Count == 0)
                {
                    queuesTicker.Stop();
                }
            }
            return base.OnDisconnected(stopCalled);
        }
    }
}