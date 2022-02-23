using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using DAL;
using DAL.DTOs;
using DAL.DTOs.Summary;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<AppUser> GetUserAsync(string username)
        {
            return await _context
                .Users
                .Include(x => x.EmailPrefrence)
                .Include(x => x.Photo)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<UserSummaryDto>
        GetUserSummaryDtoAsync(string username)
        {
            return await _mapper
                .ProjectTo<UserSummaryDto>(_context
                    .Users
                    .Where(x => x.UserName == username))
                .SingleOrDefaultAsync();         
        }

        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            return await _context
                .Users
                .Include(x => x.EmailPrefrence)
                .Include(x => x.Photo)
                .SingleOrDefaultAsync(x => x.Email == email);
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await _mapper
                .ProjectTo<MemberDto>(_context.Users)
                .ToListAsync();
        }

        public async void ChangeCurrentUserOnlineStatus(
            string username,
            bool isOnline
        )
        {
            var user =
                await _context
                    .Users
                    .SingleOrDefaultAsync(x => x.UserName == username);
            user.IsOnline = isOnline;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _mapper
                .ProjectTo<MemberDto>(_context
                    .Users
                    .Where(x => x.UserName == username))
                .SingleOrDefaultAsync();
        }

        // public async Task<int> GetUserId(string username)
        // {
        //     return await _context.Users.SingleOrDefaultAsync(x => x.UserName == username).Id;
        // }
    }
}
