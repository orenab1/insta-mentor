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
    public class CommonRepository : ICommonRepository
    {
        private readonly DataContext _context;

        private readonly IMapper _mapper;

        public CommonRepository(DataContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<int> AddPhoto(Photo photo)
        {
            await _context.Photos.AddAsync(photo);

            await _context.SaveChangesAsync();

            return  photo.Id;
        }

        public async Task<string> GetPhotoUrl(int id)
        {
            var photo=await _context.Photos.SingleOrDefaultAsync(x => x.Id == id);

            if (photo!=null)
            {
                return photo.Url;
            }
            return string.Empty;
        }
    }
}
