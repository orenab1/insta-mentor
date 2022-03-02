using System;

namespace DAL.DTOs
{
    public class QuestionSummaryDto
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public int NumOfComments { get; set; }
        public int NumOfOffers { get; set; }
        public string AskerUsername { get; set; }
        public string AskerPhotoUrl { get; set; }

        public TagDto[] Tags { get; set; }

        // Used to calculate HasCommonCommunities.
        public CommunityDto[] Communities { get; set; }

        public bool IsPayed { get; set; }
        public bool HasCommonTags { get; set; }
        public bool HasCommonCommunities { get; set; }

        public string HowLongAgo { get; set; }

        public int Length { get; set; }

        // Used for ordering
        public DateTime Created { get; set; }

        
        public bool IsActive { get; set; }
        public bool IsSolved { get; set; }

        
    }
}