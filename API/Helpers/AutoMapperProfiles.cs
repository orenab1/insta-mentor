using System.ComponentModel;
using System.Runtime.InteropServices;
using API.DTOs;
using AutoMapper;
using DAL.Entities;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                    src.Photo.Url));
            CreateMap<Photo, PhotoDto>();
        }
    }
}