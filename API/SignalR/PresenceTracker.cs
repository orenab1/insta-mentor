using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DAL.Repositories;
using DAL.Interfaces;
using DAL.DTOs.Full;
using API.Interfaces;

namespace API.SignalR
{
    public class PresenceTracker
    {
         private readonly IUnitOfWork _unitOfWork;


        public PresenceTracker( IUnitOfWork unitOfWork)
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

        public Task<UserConnectedDurationDto[]> GetOnlineUsersWithTimes()
        {
            return Task.FromResult(_unitOfWork.UserRepository.GetOnlineUsersWithTimes());
        }

        public Task<bool> IsUserOnline(string username){
            return Task.FromResult(_unitOfWork.UserRepository.IsUserOnline(username));
        }

        public Task<List<string>> GetConnectionsForUser(int userId)
        {
            return Task.FromResult(_unitOfWork.UserRepository.GetConnectionIdsForUser(userId));
        }
    }
}
