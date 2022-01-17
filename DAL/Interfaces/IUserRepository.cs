using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using DAL.DTOs;
using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<IEnumerable<AppUser>> GetUsersAsync();

        Task<AppUser> GetUserAsync(string username);

        Task<IEnumerable<MemberDto>> GetMembersAsync();

        Task<MemberDto> GetMemberAsync(string username);

        void ChangeCurrentUserOnlineStatus(string username, bool isOnline);       
    }
}