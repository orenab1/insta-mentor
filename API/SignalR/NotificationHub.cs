using System;
using System.Threading.Tasks;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class NotificationsHub : Hub
    {
        private readonly PresenceTracker _tracker;

        public NotificationsHub(PresenceTracker tracker)
        {
            _tracker = tracker;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients
                .Others
                .SendAsync("UserIsOnline", Context.User.GetUsername());

            await Clients.Caller.SendAsync("GetOnlineUsers", null);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients
                .Others
                .SendAsync("UserIsOffline", Context.User.GetUsername());

            await base.OnDisconnectedAsync(exception);
        }

        public async Task NewOfferReceived(string askerUsername, int questionId)
        {
            var connections =
                await _tracker.GetConnectionsForUser(askerUsername);

            if (connections != null)
            {
                await Clients
                    .Clients(connections)
                    .SendAsync("NewOfferReceived",
                    new { questionId = questionId });
            }
        }
    }
}
