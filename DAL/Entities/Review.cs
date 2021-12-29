using System;

namespace DAL.Entities
{
    public class Review
    {
        public int QuestionId { get; set; }
        public float Rating { get; set; }
        public string Text { get; set; }
        public int ReviewerId { get; set; }
        public int RevieweeId { get; set; }
        public DateTime Created { get; set; }

    }
}