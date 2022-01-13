namespace DAL.Entities
{
    public class UsersTags
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}