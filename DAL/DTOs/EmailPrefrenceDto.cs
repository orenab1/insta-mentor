namespace DAL.DTOs
{
    public class EmailPrefrenceDto
    {
        public bool QuestionAskedOnMyTags { get; set; }
        public bool OnlyNotifyOnCommunityQuestionAsked { get; set; }
        public bool MyQuestionReceivedNewOffer { get; set; }
        public bool MyQuestionReceivedNewComment { get; set; }
    }
}