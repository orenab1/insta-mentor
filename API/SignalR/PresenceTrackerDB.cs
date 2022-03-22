using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DAL.Repositories;
using DAL.Interfaces;

namespace API.SignalR
{
    public class PresenceTrackerDB
    {
         private readonly IUnitOfWork _unitOfWork;

        public PresenceTrackerDB( IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task
        UserConnected(string username, string connectionId, string userAgent)
        {
            await _unitOfWork.UserRepository
                .SaveNewConnectionForUser(username, connectionId, userAgent);
        }

        public async Task<bool> UserDisconnected(string connectionId)
        {
           return await _unitOfWork.UserRepository
                .MarkConnectionClosed(connectionId);
        }

        public Task<string[]> GetOnlineUsers()
        {
            return Task.FromResult(_unitOfWork.UserRepository.GetOnlineUsers());
        }

        public Task<bool> IsUserOnline(string username){
            return Task.FromResult(_unitOfWork.UserRepository.IsUserOnline(username));
        }

        // public Task<List<string>> GetConnectionsForUser(string username)
        // {
        // }
    }
}
