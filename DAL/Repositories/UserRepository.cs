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

        public  List<string> GetConnectionIdsForUser(int userId){
            return  _context.Connections.Where(c=>c.User.Id==userId && !c.DisconnectedTime.HasValue).Select(c=>c.ConnectionID).ToList();
        }


         public async Task<bool>
        MarkConnectionClosed(
            string connectionId
        )
        {
            var connection =_context.Connections.Find(connectionId);
            connection.DisconnectedTime=DateTime.Now;
            return await _context.SaveChangesAsync()>0;
        }

        public string[]
        GetOnlineUsers(
        )
        {
            return _context.Connections.Where(c=>!c.DisconnectedTime.HasValue).Select(c=>c.User.UserName).Distinct().ToArray();
        }

        public bool IsUserOnline(string username){

            return _context.Users.Include(u => u.Connections).SingleOrDefault(u=>u.UserName == username).Connections.Any(c=>!c.DisconnectedTime.HasValue && DateTime.Now.Subtract(c.ConnectedTime).TotalDays<=1);
        }

       
        public async Task<bool>
        SaveNewConnectionForUser(
            string username,
            string connectionId,
            string userAgent
        )
        {
            var user =
                _context
                    .Users
                    .Include(u => u.Connections)
                    .SingleOrDefault(u => u.UserName == username);

            user
                .Connections
                .Add(new Connection {
                    ConnectionID = connectionId,
                    UserAgent = userAgent,
                    ConnectedTime = DateTime.Now
                });

           return await _context.SaveChangesAsync()>0;
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

        public async Task<UserSummaryDto> GetUserSummaryDtoById(int id)
        {
            return await _mapper
                .ProjectTo<UserSummaryDto>(_context
                    .Users
                    .Where(x => x.Id == id))
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

        public async Task<IEnumerable<UserFullDto>> GetMembersAsync()
        {
            return await _mapper
                .ProjectTo<UserFullDto>(_context.Users)
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

        public async Task<UserFullDto> GetUserAsync(int userId)
        {
            return await _mapper
                .ProjectTo<UserFullDto>(_context
                    .Users
                    .Where(x => x.Id == userId))
                .SingleOrDefaultAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int userId)
        {
            return await _context
                .Users
                .Include(x => x.EmailPrefrence)
                .Where(x => x.Id == userId)
                .SingleOrDefaultAsync();
        }

        public async Task<bool>
        UpdateCommunitiesForUser(CommunityDto[] newCommunities, int userId)
        {
            int[] allCommunitiesIds =
                newCommunities.Select(x => x.Value).ToArray();

            UsersCommunities[] allUserCommunities =
                _context
                    .UsersCommunities
                    .Where(uc => uc.AppUserId == userId)
                    .ToArray();

            foreach (UsersCommunities uc in allUserCommunities)
            {
                _context.Entry(uc).State = EntityState.Deleted;
            }

            await _context.SaveChangesAsync();

            foreach (CommunityDto communityDto in newCommunities)
            {
                _context
                    .UsersCommunities
                    .Add(new UsersCommunities {
                        AppUserId = userId,
                        CommunityId = communityDto.Value
                    });
            }

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
