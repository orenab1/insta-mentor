using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using DAL.DTOs;
using DAL.DTOs.Full;
using DAL.DTOs.Summary;
using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);

        Task<AppUser> GetUserAsync(string username);

        Task<IEnumerable<UserFullDto>> GetMembersAsync();

        Task<UserFullDto> GetUserAsync(int userId);

        void ChangeCurrentUserOnlineStatus(string username, bool isOnline);

        Task<AppUser> GetUserByEmailAsync(string email);

        Task<UserSummaryDto> GetUserSummaryDtoAsync(string username);

        Task<UserSummaryDto> GetUserSummaryDtoById(int id);

        Task<AppUser> GetUserByIdAsync(int userId);

        Task<bool>
        UpdateCommunitiesForUser(
            CommunityDto[] newCommunities, int userId
        );

        Task<bool>
        SaveNewConnectionForUser(
            string username,
            string connectionId,
            string userAgent
        );

        Task<bool>
        MarkConnectionClosed(
            string connectionId
        );

        string[] GetOnlineUsers();

        UserConnectedDurationDto[] GetOnlineUsersWithTimes();

        public bool IsUserOnline(string username);

        public  List<string> GetConnectionIdsForUser(int userId);

        Task<bool> MarkUserAsVerified(int userId);
    }
}
