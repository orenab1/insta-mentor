using System;

namespace DAL.DTOs
{
    public class MyQuestionSummaryDto 
    {
       public int Id { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public int NumOfComments { get; set; }
        public int NumOfOffers { get; set; }

        public TagDto[] Tags { get; set; }

        public bool IsPayed { get; set; }

        public string HowLongAgo { get; set; }

        // Used for ordering
        public DateTime Created { get; set; }
        public bool IsActive { get; set; }
        public bool IsSolved { get; set; }
        public bool HasAtLeastOneOnlineOfferer { get; set; }
    }
}