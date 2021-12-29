using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using DAL.Entities;

namespace API.Interfaces.Repositories
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();

        Task<AppUser> GetUserAsync(int id);

        Task<MemberDto> GetMembersAsync();


    }
}