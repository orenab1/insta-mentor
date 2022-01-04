using System;

namespace DAL.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public float Rating { get; set; }
        public string Text { get; set; }
        public int ReviewerId { get; set; }
        public int RevieweeId { get; set; }
        public DateTime Created { get; set; }

        public bool IsRevieweeAnswerer { get; set; }

        public AppUser Reviewer { get; set; }
        public AppUser Reviewee { get; set; }

        public Question Question { get; set; }
    }
}