using System.Configuration;
using DAL.Interfaces;
using DAL.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using DAL;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using DAL.DTOs;
using System.Linq;
using System;

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

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<AppUser> GetUserAsync(string username)
        {
            return await _context.Users
                .Include(x => x.EmailPrefrence)
                .Include(x => x.Photo)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await _mapper
                .ProjectTo<MemberDto>(_context.Users)
                .ToListAsync();
        }


        public async void ChangeCurrentUserOnlineStatus(string username, bool isOnline)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == username);
            user.IsOnline = isOnline;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == username);

            // return new MemberDto()
            // {
            //     Id = user.Id,
            //     AboutMe = user.AboutMe,
            //     Email = user.Email,
            //     Title = user.Title,
            //     Username = user.UserName
            // };
            // return await _context.Users.SingleOrDefaultAsync(x => x.UserName == username)


            return await _mapper
                .ProjectTo<MemberDto>(_context.Users.Where(x => x.UserName == username))
                .SingleOrDefaultAsync();

        }

        

        


        // public async Task<int> GetUserId(string username)
        // {
        //     return await _context.Users.SingleOrDefaultAsync(x => x.UserName == username).Id;
        // }
    }
}