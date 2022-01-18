namespace DAL.Entities
{
    public class QuestionsCommunities
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int CommunityId { get; set; }
        public Community Community { get; set; }
    }
}