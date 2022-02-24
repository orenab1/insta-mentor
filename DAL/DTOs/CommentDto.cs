namespace DAL.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int QuestionId { get; set; }
        public int CommentorId { get; set; }

        public string HowLongAgo { get; set; }
    }
}