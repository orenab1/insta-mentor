using System;
using System.Threading.Tasks;
using System.Web;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using DAL.DTOs.Full;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class PresenceHub : Hub<IPresenceHub>
    {
        private readonly PresenceTracker _tracker;

        public PresenceHub(PresenceTracker tracker)
        {
            _tracker = tracker;
        }

        public override async Task OnConnectedAsync()
        {
            await _tracker
                .UserConnected(Context.User.GetUsername(),
                Context.ConnectionId,
                string.Empty);

            await Clients.Others.UserIsOnline(Context.User.GetUsername());

            var currentUsers = await _tracker.GetOnlineUsers();
            await Clients.Caller.GetOnlineUsers(currentUsers);

            UserConnectedDurationDto[] currentUsersWithTime =
                await _tracker.GetOnlineUsersWithTimes();
            await Clients.Caller.GetOnlineUsersWithTimes(currentUsersWithTime);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await _tracker.UserDisconnected(Context.ConnectionId);

            bool isOnline =
                await _tracker.IsUserOnline(Context.User.GetUsername());

            if (!isOnline)
                await Clients.Others.UserIsOffline(Context.User.GetUsername());

            UserConnectedDurationDto[] currentUsersWithTime =
                await _tracker.GetOnlineUsersWithTimes();

            await Clients.Caller.GetOnlineUsersWithTimes(currentUsersWithTime);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
