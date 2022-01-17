namespace DAL.DTOs
{
    public class MemberUpdateDto
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Title { get; set; }

        public string AboutMe { get; set; }    

        public TagDto[] Tags { get; set; }  

        public CommunityDto[] Communities { get; set; }

        public bool QuestionAskedOnMyTags { get; set; }
        public bool OnlyNotifyOnCommunityQuestionAsked { get; set; }
        public bool MyQuestionReceivedNewOffer { get; set; }
        public bool MyQuestionReceivedNewComment { get; set; }
    }
}