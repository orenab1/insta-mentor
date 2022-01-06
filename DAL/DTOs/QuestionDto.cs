using System.Collections.Generic;
using DAL.Entities;

namespace DAL.DTOs
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Header { get; set; }

        public string Body { get; set; }

        public bool IsSolved { get; set; }

        public List<CommentDto> Comments { get; set; }
        public List<OfferInQuestionDto> Offers { get; set; }

        public int AskerId { get; set; }

        public string AskerUsername { get; set; }
    }
}