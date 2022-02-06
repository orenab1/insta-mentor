namespace DAL.DTOs
{
    public class CommunitySummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumOfMembers { get; set; }
        public int NumOfQuestionsAsked { get; set; }
        public string BestTimeToGetAnswer { get; set; }
    }
}