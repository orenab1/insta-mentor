using System;
using DAL.Entities;

namespace DAL.DTOs
{
    public class MemberDto
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Title { get; set; }

        public string AboutMe { get; set; }

        public string PhotoUrl { get; set; }

        public DateTime Created { get; set; }

        //public EmailPrefrence EmailPrefrence { get; set; }
    }
}