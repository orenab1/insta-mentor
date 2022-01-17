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
            MapUser();
            MapQuestion();

            CreateMap<Photo, PhotoDto>();

            CreateMap<Tag, TagDto>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src =>
                    src.Id))
                .ForMember(dest => dest.Display, opt => opt.MapFrom(src =>
                    src.Text));

             CreateMap<TagDto, Tag>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src =>
                    src.Value))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src =>
                    src.Display));

            CreateMap<Community, CommunityDto>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src =>
                    src.Id))
                .ForMember(dest => dest.Display, opt => opt.MapFrom(src =>
                    src.Name));


            CreateMap<CommunityDto, Community>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src =>
                    src.Value))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                    src.Display));

        }

        private void MapUser()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                    src.Photo.Url))
                .ForMember(dest => dest.PhotoId, opt => opt.MapFrom(src =>
                    src.Photo.Id));

            CreateMap<MemberUpdateDto, AppUser>();

            CreateMap<EmailPrefrenceDto, EmailPrefrence>();
            CreateMap<EmailPrefrence, EmailPrefrenceDto>();

            CreateMap<UsersTags, TagDto>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src =>
                    src.Tag.Id))
                .ForMember(dest => dest.Display, opt => opt.MapFrom(src =>
                    src.Tag.Text));


             CreateMap<TagDto, UsersTags>()
                .ForMember(dest=>dest.Tag,
                    opt=>opt.MapFrom(src => src));

            CreateMap<CommunityDto, UsersCommunities>()
                .ForMember(dest=>dest.Community,
                    opt=>opt.MapFrom(src => src));

            CreateMap<UsersCommunities, CommunityDto>()
               .ForMember(dest => dest.Value, opt => opt.MapFrom(src =>
                   src.Community.Id))
               .ForMember(dest => dest.Display, opt => opt.MapFrom(src =>
                   src.Community.Name));

            // CreateMap<CommunityDto, UsersCommunities>()
            //    .ForMember(dest => dest.Community.Id, opt => opt.MapFrom(src =>
            //        src.Value))
            //    .ForMember(dest => dest.Community.Name, opt => opt.MapFrom(src =>
            //        src.Display));

        }


        private void MapQuestion()
        {
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