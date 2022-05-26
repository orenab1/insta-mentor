using DAL.Entities;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IAccountRepository
    {
        Task<bool> CreateUserAsync(AppUser user);

        Task<bool> IsUserExistsAsync(string email);

        Task<AppUser> CreateFakeUserAsync(string email);
    }
}