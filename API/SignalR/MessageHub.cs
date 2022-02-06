using System;
using System.Threading.Tasks;
using API.Extensions;
using API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class MessageHub : Hub
    {
        private readonly PresenceTracker _presenceTracker;

        private readonly PresenceHub _presenceHub;

        public MessageHub( 
            PresenceTracker presenceTracker,
            PresenceHub presenceHub
        )
        {
            this._presenceTracker = presenceTracker;
            this._presenceHub = presenceHub;
        }

        // public async Task SendMessage()
        // {
        //     var connections =
        //         await _presenceTracker.GetConnectionsForUser("dave");
        //     if (connections != null)
        //     {
        //         await _presenceHub
        //             .Clients
        //             .Clients(connections)
        //             .SendAsync("NewMessageReceived",
        //             new { username = "post1" });
        //     }

        //     await _presenceHub
        //         .Clients
        //         .Clients(connections)
        //         .SendAsync("NewMessage", "yo");
        // }
    }
}
