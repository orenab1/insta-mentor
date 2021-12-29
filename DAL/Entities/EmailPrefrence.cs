using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace DAL.Entities
{
    [Table("EmailPrefrences")]
    public class EmailPrefrence
    {
        [Key]
        [ForeignKey("AppUser")]
        public int AppUserId { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public bool WantToRecieveEmails { get; set; }

        public AppUser AppUser { get; set; }

    }
}