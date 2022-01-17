namespace DAL.Entities
{
    public class UsersCommunities
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int CommunityId { get; set; }
        public Community Community { get; set; }
    }
}