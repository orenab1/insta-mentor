using System;

namespace DAL.DTOs
{
    public class CommunityFullDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int NumOfMembers { get; set; }

        public int NumOfQuestionsAsked { get; set; }

        public string BestTimeToGetAnswer { get; set; }

        public bool IsCurrentUserMember { get; set; }

        public bool IsCurrentUserCreator { get; set; }

        // This is used for mapping
        public int CreatorId { get; set; }

        public DateTime Created { get; set; }
    }
}
