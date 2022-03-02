using System;
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

        public bool IsPayed { get; set; }

        public string AnsweredBy { get; set; }

        public string LastOffererUsername { get; set; }

        public bool IsActive { get; set; }

        public int Length { get; set; }

        public DateTime Created { get; set; }

        public List<CommentDto> Comments { get; set; }

        public List<OfferInQuestionDto> Offers { get; set; }

        public TagDto[] Tags { get; set; }

        public CommunityDto[] Communities { get; set; }

        public bool IsCurrentUserQuestionAsker { get; set; }

        public string AskerUsername { get; set; }

        public string AskerTitle { get; set; }
        public int AskerId { get; set; }

        public float AskerAverageRating { get; set; }

        public float AskerNumOfRatings { get; set; }

        public int? PhotoId { get; set; }

        public string PhotoUrl { get; set; }
    }
}
