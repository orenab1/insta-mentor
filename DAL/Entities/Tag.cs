using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    [Table("Tags")]
    public class Tag
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsApproved { get; set; }
        public int? CreatorId { get; set; }
        public AppUser Creator { get; set; }

        public ICollection<UsersTags> Users { get; set; }
    }
}