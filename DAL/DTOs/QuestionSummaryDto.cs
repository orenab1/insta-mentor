namespace DAL.DTOs
{
    public class QuestionSummaryDto
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public int NumOfComments { get; set; }
        public int NumOfOffers { get; set; }
        public int AskerId { get; set; }
        public string AskerUsername { get; set; }
        public string AskerPhotoUrl { get; set; }
        public string AskerPhotoId { get; set; }
    }
}