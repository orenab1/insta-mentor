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
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace API.SignalR
{
    [Authorize]
    public class PresenceHub : Hub<IPresenceHub>
    {
        private readonly PresenceTracker _tracker;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public PresenceHub(PresenceTracker tracker,IHttpContextAccessor httpContextAccessor)
        {
            _tracker = tracker;
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task OnConnectedAsync()
        {
            var userAgent= _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.UserAgent].ToString();


            await _tracker
                .UserConnected(Context.User.GetUsername(),
                Context.ConnectionId,
                userAgent);

            
            await Clients.Others.UserIsOnline(Context.User.GetUsername());

            var currentUsers = await _tracker.GetOnlineUsers();
            await Clients.Caller.GetOnlineUsers(currentUsers);

            UserConnectedDurationDto[] currentUsersWithTime =
                await _tracker.GetOnlineUsersWithTimes();
             await Clients.All.GetOnlineUsersWithTimes(currentUsersWithTime);
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

            await Clients.All.GetOnlineUsersWithTimes(currentUsersWithTime);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
