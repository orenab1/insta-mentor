using System.ComponentModel;
using System.Runtime.InteropServices;
using DAL.DTOs;
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
                    src.Photo.Url))
                .ForMember(dest => dest.PhotoId, opt => opt.MapFrom(src =>
                    src.Photo.Id));

            CreateMap<MemberUpdateDto,AppUser>();
            
            CreateMap<Photo, PhotoDto>();
            CreateMap<Question, QuestionDto>()
                .ForMember(dest => dest.AskerUsername, opt => opt.MapFrom(src =>
                    src.Asker.UserName));

            CreateMap<Question, QuestionSummaryDto>()
                .ForMember(dest => dest.AskerUsername, opt => opt.MapFrom(src =>
                    src.Asker.UserName))
                .ForMember(dest => dest.NumOfOffers, opt => opt.MapFrom(src =>
                    src.Offers.Count))
                .ForMember(dest => dest.NumOfComments, opt => opt.MapFrom(src =>
                    src.Comments.Count));
            
            CreateMap<Comment, CommentDto>();
            CreateMap<Offer, OfferInQuestionDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src =>
                    src.Offerer.UserName));
        }
    }
}