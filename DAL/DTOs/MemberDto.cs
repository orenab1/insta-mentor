using System;
using DAL.Entities;

namespace DAL.DTOs
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }

        public string Title { get; set; }

        public string AboutMe { get; set; }

        public int? PhotoId { get; set; }

        public string PhotoUrl { get; set; }

    //    public DateTime Created { get; set; }

        public bool IsOnline { get; set; }

        public TagDto[] Tags { get; set; }

        public CommunityDto[] Communities { get; set; }

        public ReviewDto[] Reviews { get; set; }

        public EmailPrefrenceDto EmailPrefrence { get; set; }

       public float AskerAverageRating { get; set; }
       public int AskerNumOfRatings { get; set; }
    }
}