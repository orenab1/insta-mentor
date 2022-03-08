using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using AutoMapper;
using DAL.DTOs;
using DAL.DTOs.Summary;
using DAL.Entities;
using DAL.Extensions;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        private readonly CultureInfo
            CULTURE = CultureInfo.GetCultureInfo("en-US");

        public AutoMapperProfiles()
        {
            MapUser();
            MapQuestion();

            CreateMap<Photo, PhotoDto>();
            CreateMap<PhotoDto, Photo>();

            CreateMap<Tag, TagDto>()
                .ForMember(dest => dest.Value,
                opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Display,
                opt => opt.MapFrom(src => src.Text));

            CreateMap<TagDto, Tag>()
                .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.Value))
                .ForMember(dest => dest.Text,
                opt => opt.MapFrom(src => src.Display));

            CreateMap<Community, CommunityDto>()
                .ForMember(dest => dest.Value,
                opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Display,
                opt => opt.MapFrom(src => src.Name));

            CreateMap<Community, CommunityFullDto>()
                .ForMember(dest => dest.NumOfQuestionsAsked,
                opt =>
                    opt
                        .MapFrom(src =>
                            src
                                .Questions
                                .Where(x => x.Question.IsActive)
                                .Count()))
                .ForMember(dest => dest.NumOfMembers,
                opt => opt.MapFrom(src => src.Users.Count));

            CreateMap<CommunityDto, Community>()
                .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.Value))
                .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.Display));
        }

        private void MapUser()
        {
            CreateMap<AppUser, UserSummaryDto>()
                .ForMember(dest => dest.PhotoUrl,
                opt => opt.MapFrom(src => src.Photo.Url));

            CreateMap<AppUser, UserFullDto>()
                .ForMember(dest => dest.PhotoUrl,
                opt => opt.MapFrom(src => src.Photo.Url))
                .ForMember(dest => dest.PhotoId,
                opt => opt.MapFrom(src => src.Photo.Id))
                .ForMember(dest => dest.Reviews,
                opt => opt.MapFrom(src => src.ReviewsReceived))
                .ForMember(dest => dest.AskerNumOfRatings,
                opt =>
                    opt
                        .MapFrom(src =>
                            src.ReviewsReceived != null
                                ? src.ReviewsReceived.Count
                                : 0))
                .ForMember(dest => dest.AskerAverageRating,
                opt =>
                    opt
                        .MapFrom(src =>
                            (src.ReviewsReceived == null || src.ReviewsReceived.Count == 0)
                                ? 0
                                : (float)
                                src.ReviewsReceived.Average(r => r.Rating)));

            //             (src.ReviewsReceived != null && src.ReviewsReceived.Count !=0)?
            // ((float)src.ReviewsReceived.Sum(r => r.Rating) / (float)src.ReviewsReceived.Count)
            // : 0));
            // (src.ReviewsReceived == null || src.ReviewsReceived.Count==0)
            //     ? 0
            //     : (
            //     (float) (src.ReviewsReceived.Sum(r => r.Rating) /
            //     (float) src.ReviewsReceived.Count)
            //     )));
            CreateMap<MemberUpdateDto, AppUser>();

            CreateMap<EmailPrefrenceDto, EmailPrefrence>();
            CreateMap<EmailPrefrence, EmailPrefrenceDto>();

            CreateMap<UsersTags, TagDto>()
                .ForMember(dest => dest.Value,
                opt => opt.MapFrom(src => src.Tag.Id))
                .ForMember(dest => dest.Display,
                opt => opt.MapFrom(src => src.Tag.Text));

            CreateMap<TagDto, UsersTags>()
                .ForMember(dest => dest.Tag, opt => opt.MapFrom(src => src));

            CreateMap<CommunityDto, UsersCommunities>()
                .ForMember(dest => dest.Community,
                opt => opt.MapFrom(src => src));

            CreateMap<UsersCommunities, CommunityDto>()
                .ForMember(dest => dest.Value,
                opt => opt.MapFrom(src => src.Community.Id))
                .ForMember(dest => dest.Display,
                opt => opt.MapFrom(src => src.Community.Name));

            // CreateMap<CommunityDto, UsersCommunities>()
            //    .ForMember(dest => dest.Community.Id, opt => opt.MapFrom(src =>
            //        src.Value))
            //    .ForMember(dest => dest.Community.Name, opt => opt.MapFrom(src =>
            //        src.Display));
        }

        private void MapQuestion()
        {
            CreateMap<QuestionEditDto, Question>()
                .ForMember(dest => dest.Created, opt => opt.Ignore())
                .ForMember(dest => dest.Tags, opt => opt.Ignore())
                .ForMember(dest => dest.Communities, opt => opt.Ignore());

            // .ForMember(dest => dest.Photo.Url,
            // opt => opt.MapFrom(src => src.PhotoUrl))
            // .ForMember(dest => dest.Photo.Id,
            // opt => opt.MapFrom(src => src.PhotoId));
            CreateMap<Question, QuestionDto>()
                .ForMember(dest => dest.AskerUsername,
                opt => opt.MapFrom(src => src.Asker.UserName))
                .ForMember(dest => dest.Comments,
                opt => opt.MapFrom(src => src.Comments.Where(x => x.IsActive)))
                .ForMember(dest => dest.PhotoUrl,
                opt => opt.MapFrom(src => src.Photo.Url))
                .ForMember(dest => dest.PhotoId,
                opt => opt.MapFrom(src => src.Photo.Id));
            CreateMap<Question, QuestionSummaryDto>()
                .ForMember(dest => dest.AskerUsername,
                opt => opt.MapFrom(src => src.Asker.UserName))
                .ForMember(dest => dest.AskerPhotoUrl,
                opt => opt.MapFrom(src => src.Asker.Photo.Url))
                .ForMember(dest => dest.NumOfOffers,
                opt => opt.MapFrom(src => src.Offers.Count))
                .ForMember(dest => dest.NumOfComments,
                opt =>
                    opt
                        .MapFrom(src =>
                            src.Comments.Where(x => x.IsActive).Count()))
                .ForMember(dest => dest.HowLongAgo,
                opt => opt.MapFrom(src => src.Created.AsLongAgo()));

            CreateMap<Question, MyQuestionSummaryDto>()
                .ForMember(dest => dest.NumOfOffers,
                opt => opt.MapFrom(src => src.Offers.Count))
                .ForMember(dest => dest.NumOfComments,
                opt =>
                    opt
                        .MapFrom(src =>
                            src.Comments.Where(x => x.IsActive).Count()))
                .ForMember(dest => dest.HowLongAgo,
                opt => opt.MapFrom(src => src.Created.AsLongAgo()))
                .ForMember(dest => dest.HasAtLeastOneOnlineOfferer,
                opt => opt.Ignore());

            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.HowLongAgo,
                opt => opt.MapFrom(src => src.Created.AsLongAgo()));
            CreateMap<Offer, OfferInQuestionDto>()
                .ForMember(dest => dest.HowLongAgo,
                opt => opt.MapFrom(src => src.Created.AsLongAgo()))
                .ForMember(dest => dest.Username,
                opt => opt.MapFrom(src => src.Offerer.UserName));

            CreateMap<TagDto, QuestionsTags>()
                .ForMember(dest => dest.TagId,
                opt => opt.MapFrom(src => src.Value));

            CreateMap<CommunityDto, QuestionsCommunities>()
                .ForMember(dest => dest.CommunityId,
                opt => opt.MapFrom(src => src.Value));

            CreateMap<QuestionsTags, TagDto>()
                .ForMember(dest => dest.Value,
                opt => opt.MapFrom(src => src.Tag.Id))
                .ForMember(dest => dest.Display,
                opt => opt.MapFrom(src => src.Tag.Text));

            CreateMap<QuestionsCommunities, CommunityDto>()
                .ForMember(dest => dest.Value,
                opt => opt.MapFrom(src => src.Community.Id))
                .ForMember(dest => dest.Display,
                opt => opt.MapFrom(src => src.Community.Name));

            CreateMap<ReviewDto, Review>();
            CreateMap<Review, ReviewDto>();
        }
    }
}
