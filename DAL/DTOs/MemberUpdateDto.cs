using DAL.Entities;

namespace DAL.DTOs
{
    public class MemberUpdateDto
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Title { get; set; }

        public string AboutMe { get; set; }    

        public TagDto[] Tags { get; set; }  

        public CommunityDto[] Communities { get; set; }

        public EmailPrefrence EmailPrefrence { get; set; }
    }
}