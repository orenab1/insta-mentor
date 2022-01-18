using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    [Table("Reviews")]
    public class Review
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int Rating { get; set; }
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