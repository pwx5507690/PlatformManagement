using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace PlatformManagement.Hubs
{
    public class Chat : Hub
    {
        public void Receives(Message message)
        {
            NoticeQueue.Get.Notify(message);
            Clients.All.sendMessage(message);
        }
        public void Send(Message message)
        {
            Clients.All.sendMessage(message);
        }

        public static IHubContext GetHubContext()
        {
            return GlobalHost.ConnectionManager.GetHubContext<Chat>();
        }

        public static void SendClientMessage(Message message)
        {
            Chat.GetHubContext().Clients.All.sendMessage(message);
        }
      
    }
}