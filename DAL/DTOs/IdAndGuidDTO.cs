namespace DAL.DTOs
{
    public class IdAndGuidDTO
    {
        public int Id { get; set; }
        public string Guid { get; set; }

        public string GetIdOrGuid(){
            return string.IsNullOrEmpty(Guid)?
                Id.ToString():
                Guid;
        }
    }
}