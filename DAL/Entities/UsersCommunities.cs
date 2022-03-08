using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class UsersCommunities
    {
        public int Id { get; set; }
        public int? AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
        public int? CommunityId { get; set; }
        [ForeignKey("CommunityId")]
        public Community Community { get; set; }
    }
}