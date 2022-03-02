using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using DAL.DTOs;
using DAL.DTOs.Summary;
using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);

        Task<AppUser> GetUserAsync(string username);

        Task<IEnumerable<MemberDto>> GetMembersAsync();

        Task<MemberDto> GetUserAsync(int userId);

        void ChangeCurrentUserOnlineStatus(string username, bool isOnline);

        Task<AppUser> GetUserByEmailAsync(string email);

        Task<UserSummaryDto> GetUserSummaryDtoAsync(string username);

        Task<UserSummaryDto> GetUserSummaryDtoById(int id);

        Task<AppUser> GetUserByIdAsync(int userId);

        Task<bool>
        UpdateCommunitiesForUser(
            CommunityDto[] newCommunities, int userId
        );
    }
}
