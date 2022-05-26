namespace DAL.Entities
{
    public class QuestionFeedbackRequestor
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int RequestorId { get; set; }
        public AppUser Requestor { get; set; }
    }
}