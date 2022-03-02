namespace DAL.DTOs.Partial
{
    public class AskerAcceptedOfferDto
    {
        public int OffererUserId { get; set; }

        public string QuestionHeader { get; set; }

        public string QuestionBody { get; set; }

        public int AskerUserId { get; set; }

        public string MeetingUrl { get; set; }

        public int QuestionId { get; set; }

        public string AskerUsername { get; set; }
    }
}
