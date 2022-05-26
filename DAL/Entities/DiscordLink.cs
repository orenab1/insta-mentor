namespace DAL.Entities
{
    public class DiscordLink
    {
        public int Id { get; set; }
        public string Link { get; set; }

        public int? QuestionId{get;set;}

        public Question Question{get;set;}
    }
}