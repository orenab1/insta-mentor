using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    [Table("Offers")]
    public class Offer
    {
        public int Id { get; set; }
        public Question Question { get; set; }
        public string Text { get; set; }
        public AppUser Offerer { get; set; }
        public DateTime Created { get; set; }
    }
}