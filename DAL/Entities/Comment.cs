using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    [Table("Comments")]
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }

        public Question Question { get; set; }
        public AppUser Commentor { get; set; }
    }
}