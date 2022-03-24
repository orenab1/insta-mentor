using System;

namespace DAL.Entities
{
    public class Connection
    {
        public AppUser User { get; set; }

        public string ConnectionID { get; set; }

        public string UserAgent { get; set; }

        public DateTime ConnectedTime { get; set; }
        public DateTime? DisconnectedTime { get; set; }
    }
}
