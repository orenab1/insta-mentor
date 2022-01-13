using System.Threading.Tasks;
using DAL.DTOs;
using AutoMapper;
using System.Linq;
using DAL.Interfaces;
using System.Collections.Generic;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CommonRepository : ICommonRepository
    {

        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CommonRepository(DataContext context, IMapper mapper)
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
    }
}