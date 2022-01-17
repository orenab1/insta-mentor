using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DAL.DTOs;
using DAL.Interfaces;
using System.Data;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;
using System;
using System.Linq;

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
                .ToListAsync();
        }

        public async Task<bool> UpdateCommunitiesForUser(CommunityDto[] newCommunities, int userId)
        {
            var CommunitiesNotCurrentlyInDB = newCommunities.Where(x => x.Value == 0);

            foreach (CommunityDto community in CommunitiesNotCurrentlyInDB)
            {
                Community newCommunity = new Community
                {
                    Created = DateTime.Now,
                    CreatorId = userId,
                    IsApproved = true,
                    Name = community.Display
                };

                _context.Communities.Add(newCommunity);
                _context.SaveChanges();

                community.Value = newCommunity.Id;
            }



            var newUserCommunities = new List<UsersCommunities>();

            if (newCommunities != null)
            {
                foreach (CommunityDto communityDto in newCommunities)
                {
                    newUserCommunities.Add(new UsersCommunities
                    {
                        AppUserId = userId,
                        CommunityId = communityDto.Value
                    });
                }
            }

            var prevUserCommunities = _context.UsersCommunities.Where(i => i.AppUserId == userId).ToList();

            if (prevUserCommunities != null && prevUserCommunities.Count > 0) _context.UsersCommunities.RemoveRange(prevUserCommunities);

            if (newUserCommunities.Count > 0) _context.UsersCommunities.AddRange(newUserCommunities);

            return _context.SaveChanges() > 0;
        }
    }
}