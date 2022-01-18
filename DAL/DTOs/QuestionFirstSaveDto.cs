

using System.Collections.Generic;

namespace DAL.DTOs
{
    public class QuestionFirstSaveDto
    {
        // For redirect after first save
        public int Id { get; set; }

        // No need to send from client. Retrieved in server
        public int AskerId { get; set; }
        public string Header { get; set; }

        public string Body { get; set; }

        public List<TagDto> Tags { get; set; }
        public List<CommunityDto> Communities { get; set; }
    }
}