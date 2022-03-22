using System;
using System.Threading.Tasks;
using API.Extensions;
using API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using API.Interfaces;
using System.Web;

namespace API.SignalR
{
    [Authorize]
    public class PresenceHub : Hub<IPresenceHub> 
    {
        private readonly PresenceTrackerDB _tracker;
        public PresenceHub(PresenceTrackerDB tracker)
        {
            _tracker = tracker;
        }
        public override async Task OnConnectedAsync()
        {
            await _tracker.UserConnected(Context.User.GetUsername(), Context.ConnectionId,string.Empty);
            
                await Clients.Others.UserIsOnline(Context.User.GetUsername());

            var currentUsers = await _tracker.GetOnlineUsers();
            await Clients.Caller.GetOnlineUsers(currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await _tracker.UserDisconnected(Context.ConnectionId);

            bool isOnline= await _tracker.IsUserOnline(Context.User.GetUsername());

            if (!isOnline)
                await Clients.Others.UserIsOffline(Context.User.GetUsername());

            await base.OnDisconnectedAsync(exception);
        }

      
    }
}
