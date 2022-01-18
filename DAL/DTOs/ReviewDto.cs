namespace DAL.DTOs
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int Rating { get; set; }
        public string Text { get; set; }
        public int ReviewerId { get; set; }
        public int RevieweeId { get; set; }

    }
}