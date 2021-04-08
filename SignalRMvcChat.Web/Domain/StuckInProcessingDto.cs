using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRMvcChat.Web.Domain
{
    public class StuckInProcessingDto
    {
        public long? MinUnixTimeStamp { get; set; }
        public long? MaxUnixTimeStamp { get; set; }
        public int Count { get; set; }
    }
}