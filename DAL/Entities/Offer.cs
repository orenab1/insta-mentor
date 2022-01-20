using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    [Table("Offers")]
    public class Offer
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public int OffererId { get; set; }
        public AppUser Offerer { get; set; }

        public DateTime Created { get; set; }
    }
}