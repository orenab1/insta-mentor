using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using DAL.DTOs;
using DAL.Entities;

namespace DAL.Interfaces.Repositories
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();

        Task<AppUser> GetUserAsync(int id);

        Task<IEnumerable<MemberDto>> GetMembersAsync();

        Task<MemberDto> GetMemberAsync(int Id);
    }
}