

using System.Collections.Generic;

namespace DAL.DTOs
{
    public class QuestionEditDto
    {
        // For redirect after first save
        public int Id { get; set; }

        // No need to send from client. Retrieved in server
        public int AskerId { get; set; }
        public string Header { get; set; }

        public string Body { get; set; }

        public bool IsPayed { get; set; }
        public string IsActive { get; set; }

        public List<TagDto> Tags { get; set; }
        public List<CommunityDto> Communities { get; set; }

        public int Length{get;set;}

        public int? PhotoId{get;set;}
        public string PhotoUrl{get;set;}

       // public PhotoDto Photo { get; set; }
    }
}