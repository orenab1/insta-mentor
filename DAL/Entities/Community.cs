using System.Collections.Generic;
using System;

namespace DAL.Entities
{
    public class Community
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsApproved { get; set; }
        public int? CreatorId { get; set; }
        public AppUser Creator { get; set; }

        public ICollection<UsersCommunities> Users { get; set; }

        public DateTime Created { get; set; }
    }
}