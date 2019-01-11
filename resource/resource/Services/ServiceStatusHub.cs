using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Services
{
    public class ServiceStatusHub : Hub
    {
        private static IHubContext hubContext =
        GlobalHost.ConnectionManager.GetHubContext<ServiceStatusHub>();

        public static void PlacedOrdersNotification(string message)
        {
            hubContext.Clients.All.ordersPlaced(message);
        }

        public static void FinalizeOrderNotification(string message)
        {
            hubContext.Clients.All.ordersFinalize(message);
        }


    }
}