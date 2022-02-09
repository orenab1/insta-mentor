using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.DTOs;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CommunityRepository : ICommunityRepository
    {
        private readonly DataContext _context;

        private readonly IMapper _mapper;

        public CommunityRepository(DataContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<CommunityDto>> GetCommunities()
        {
            return await _mapper
                .ProjectTo<CommunityDto>(_context.Communities)
                .Where(c => c.IsActive)
                .ToListAsync();
        }

        public async Task<CommunityFullDto>
        GetCommunity(int communityId, int currentUserId)
        {
            var result =
                await _mapper
                    .ProjectTo<CommunityFullDto>(_context.Communities)
                    .Where(c => c.IsActive)
                    .SingleOrDefaultAsync(c => c.Id == communityId);

            result.IsCurrentUserCreator = result.CreatorId == currentUserId;
            result.IsCurrentUserMember =
                _context
                    .UsersCommunities
                    .Any(u =>
                        u.CommunityId == communityId &&
                        u.AppUserId == currentUserId);

            return result;
        }

        public List<CommunityFullDto>
        GetCommunitiesFull(int currentUserId)
        {
            List<CommunityFullDto> result =
                _mapper
                    .ProjectTo<CommunityFullDto>(_context.Communities)
                    .Where(c => c.IsActive)
                    .ToList();

            result
                .ForEach(x =>
                    x.IsCurrentUserCreator = x.CreatorId == currentUserId);

            result
                .ForEach(x =>
                    x.IsCurrentUserMember =
                        _context
                            .UsersCommunities
                            .Any(u =>
                                u.CommunityId == x.Id &&
                                u.AppUserId == currentUserId));

            return result;
        }

        public int GetNumOfUsersInCommunity(int communityId)
        {
            return _context
                .UsersCommunities
                .Where(u => u.CommunityId == communityId)
                .Count();
        }

        public async Task<bool> DeleteCommunity(int communityId)
        {
            var community =
                await _context
                    .Communities
                    .SingleOrDefaultAsync(c => c.Id == communityId);

            if (community != null)
            {
                community.IsActive = false;
            }

            _context
                .UsersCommunities
                .RemoveRange(_context
                    .UsersCommunities
                    .Where(u => u.CommunityId == communityId));

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> LeaveCommunity(int communityId, int userId)
        {
            var userCommunity =
                await _context
                    .UsersCommunities
                    .SingleOrDefaultAsync(u =>
                        u.CommunityId == communityId && u.AppUserId == userId);

            if (userCommunity == null) return false;

            _context.UsersCommunities.Remove (userCommunity);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> JoinCommunity(int communityId, int userId)
        {
            var userCommunity =
                await _context
                    .UsersCommunities
                    .SingleOrDefaultAsync(u =>
                        u.CommunityId == communityId && u.AppUserId == userId);

            if (userCommunity != null) return false;

            _context
                .UsersCommunities
                .Add(new UsersCommunities {
                    CommunityId = communityId,
                    AppUserId = userId
                });

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<DateTime?> LastCreatedCommunity(int userId)
        {
            var communitiesCreatedByUser =
                _context.Communities.Where(x => x.CreatorId == userId);

            if (
                communitiesCreatedByUser != null &&
                communitiesCreatedByUser.Count() > 0
            )
            {
                return communitiesCreatedByUser.Max(c => c.Created);
            }

            return null;
        }

        public async Task<bool> IsCommunityNameExists(string communityName)
        {
            return await _context
                .Communities
                .AnyAsync(c => c.Name == communityName);
        }

        public async Task<bool>
        CreateCommunity(AddCommunityDto addCommunityDto, int userId)
        {
            Community newCommunity =
                new Community {
                    Created = DateTime.Now,
                    CreatorId = userId,
                    IsActive = true,
                    Name = addCommunityDto.Name,
                    Description = addCommunityDto.Description,
                    BestTimeToGetAnswer = "<Not Yet Determined>"
                };

            await _context.Communities.AddAsync(newCommunity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
