using System.Collections.Generic;

namespace DAL.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string Header { get; set; }

        public string Body { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Offer> Offers { get; set; }
    }
}