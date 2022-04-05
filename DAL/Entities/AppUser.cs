using System;
using System.Collections.Generic;
using DAL.Entities;

namespace DAL.Entities
{
    public class AppUser
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Email { get; set; }

        public string Title { get; set; }

        public string AboutMe { get; set; }

        public Photo Photo { get; set; }

        public DateTime Created { get; set; }

        public EmailPrefrence EmailPrefrence { get; set; }

        public bool IsOnline { get; set; }

        public bool IsVerified { get; set; }

        public string VerificationCode { get; set; }

        public ICollection<Question> Questions { get; set; }

        public ICollection<Review> ReviewsGiven { get; set; }

        public ICollection<Review> ReviewsReceived { get; set; }

        public ICollection<UsersTags> Tags { get; set; }

        public ICollection<UsersCommunities> Communities { get; set; }

        public ICollection<Connection> Connections { get; set; }

        public string Password { get; set; }
    }
}
