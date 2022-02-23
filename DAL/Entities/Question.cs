using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Enums;

namespace DAL.Entities
{
    public class Question
    {
        public int Id { get; set; }

        public string Header { get; set; }

        public string Body { get; set; }

        public bool IsSolved { get; set; }

        public bool IsActive { get; set; }

        public bool IsPayed { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Offer> Offers { get; set; }

        public int? AskerId { get; set; }

        public AppUser Asker { get; set; }

        public DateTime Created { get; set; }

        public QuestionLength Length { get; set; }

        public ICollection<Review> Reviews { get; set; }

        public ICollection<QuestionsTags> Tags { get; set; }
        public ICollection<QuestionsCommunities> Communities { get; set; }

         public Photo Photo { get; set; }        
    }
}