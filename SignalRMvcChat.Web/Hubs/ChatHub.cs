using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalRMvcChat.Web.Hubs
{
    public class ChatHub : Hub
    {
        public void Hello(string name)
        {
            Clients.All.hello(name, "Hello");
        }
        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);
        }
    }
}