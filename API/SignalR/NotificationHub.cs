using System;
using System.Threading.Tasks;
using API.Extensions;
using API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly PresenceTracker _tracker;

        private readonly IHubContext<PresenceHub> _presenceHub;

        public NotificationHub(
            PresenceTracker tracker,
            IHubContext<PresenceHub> presenceHub
        )
        {
            _tracker = tracker;
            _presenceHub = presenceHub;
        }

        public async Task NewOfferReceived(string askerUsername, int questionId)
        {
            var connections =
                await _tracker.GetConnectionsForUser(askerUsername);

            if (connections != null)
            {
                await _presenceHub
                    .Clients
                    .Clients(connections)
                    .SendAsync("NewOfferReceived",
                    new { questionId = questionId });
            }
        }
    }
}
