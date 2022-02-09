namespace DAL.DTOs
{
    public class CommunityDto
    {
        public int Value { get; set; }
        public string Display { get; set; }

        // For filtering
        public bool IsActive { get; set; }
    }
}