using System.Threading.Tasks;
using DAL.DTOs;
using AutoMapper;
using System.Linq;
using DAL.Interfaces;
using System.Collections.Generic;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System;
using DAL.Entities;

namespace DAL.Repositories
{
    public class TagRepository : ITagRepository
    {

        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public TagRepository(DataContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<TagDto>> GetTagsByCreatorOrApproved(int creatorId)
        {
            return await _mapper
                .ProjectTo<TagDto>(_context.Tags.Where(x => x.CreatorId == creatorId || x.IsApproved))
                .ToListAsync();
        }

        public async Task<bool> UpdateTagsForUser(TagDto[] newTags, int userId)
        {
            var tagsNotCurrentlyInDB = newTags.Where(x => x.Value == 0);

            foreach (TagDto tag in tagsNotCurrentlyInDB)
            {
                Tag newTag = new Tag
                {
                    Created = DateTime.Now,
                    CreatorId = userId,
                    IsApproved = false,
                    Text = tag.Display
                };

                _context.Tags.Add(newTag);
                _context.SaveChanges();

                tag.Value = newTag.Id;
            }

            var newUserTags = new List<UsersTags>();

            if (newTags != null)
            {
                foreach (TagDto tagDto in newTags)
                {
                    newUserTags.Add(new UsersTags
                    {
                        AppUserId = userId,
                        TagId = tagDto.Value
                    });
                }
            }

            var prevUserTags = _context.UsersTags.Where(i => i.AppUserId == userId).ToList();

            if (prevUserTags != null && prevUserTags.Count > 0) _context.UsersTags.RemoveRange(prevUserTags);

            if (newUserTags.Count > 0) _context.UsersTags.AddRange(newUserTags);

            return _context.SaveChanges() > 0;
        }
    }
}