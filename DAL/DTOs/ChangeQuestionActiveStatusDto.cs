namespace DAL.DTOs
{
    public class ChangeQuestionActiveStatusDto
    {
        public int QuestionId { get; set; }
        public bool IsActive { get; set; }
    }
}