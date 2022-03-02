using System.Threading.Tasks;
using AutoMapper;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;

        private readonly IMapper _mapper;

        public AccountRepository(DataContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<bool> CreateUserAsync(AppUser user)
        {
            _context.Users.Add (user);

            await _context.SaveChangesAsync();

            user.UserName = "Guest" + user.Id;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsUserExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }
    }
}
