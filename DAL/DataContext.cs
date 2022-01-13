using Microsoft.EntityFrameworkCore;
using DAL.Entities;
using System.Collections.Generic;

namespace DAL
{
    public class DataContext:DbContext
    {
        public DataContext()
        {
            
        }

        public DataContext(DbContextOptions<DataContext> options):base(options)
        {

        }

        public DbSet<AppUser> Users{get;set;}

        public DbSet<Question> Questions{get;set;}

         public DbSet<Tag> Tags{get;set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Review>()
                .HasOne(s => s.Reviewer)
                .WithMany(l => l.ReviewsGiven)
                .HasForeignKey(s => s.ReviewerId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.Entity<Review>()
                .HasOne(s => s.Reviewee)
                .WithMany(l => l.ReviewsReceived)
                .HasForeignKey(s => s.RevieweeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Tag>().HasData(SeedTags());
        }



        private static List<Tag> SeedTags()
        {
            return new List<Tag>{new Tag
            {
                Id = 1,
                Text = "SQL",
                IsApproved = true,
                //Creator = null,
            },
            new Tag
            {
                Id = 2,
                Text = "Python",
                IsApproved = true,
             //   Creator = null,
            },
            new Tag
            {
                Id = 3,
                Text = "React",
                IsApproved = true,
              //  Creator = null,
            },
            new Tag
            {
                Id = 4,
                Text = "Angular",
                IsApproved = true,
             //   Creator = null,
            }};
        }
    }

    
}