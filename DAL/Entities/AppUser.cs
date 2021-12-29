using System;
using DAL.Entities;

namespace DAL.Entities
{
    public class AppUser
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public byte[] PasswordHash{get;set;}

        public byte[] PasswordSalt{get;set;}

        public string Email{get;set;}

        public string Title{get;set;}

        public string AboutMe{get;set;}

        public Photo Photo { get; set; }

        public DateTime Created { get; set; }

        public EmailPrefrence EmailPrefrence { get; set; }
    }
}