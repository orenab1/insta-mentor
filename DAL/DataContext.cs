using System;
using System.Collections.Generic;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) :
            base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }

        public DbSet<UsersTags> UsersTags { get; set; }

        public DbSet<UsersCommunities> UsersCommunities { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Community> Communities { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Offer> Offers { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<QuestionsTags> QuestionsTags { get; set; }

        public DbSet<QuestionsCommunities> QuestionsCommunities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<Review>()
                .HasOne(s => s.Reviewer)
                .WithMany(l => l.ReviewsGiven)
                .HasForeignKey(s => s.ReviewerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<Review>()
                .HasOne(s => s.Reviewee)
                .WithMany(l => l.ReviewsReceived)
                .HasForeignKey(s => s.RevieweeId)
                .OnDelete(DeleteBehavior.NoAction);

            // builder
            //     .Entity<UsersCommunities>()
            //     .HasOne(uc => uc.AppUser)
            //     .WithMany()
            //     .OnDelete(DeleteBehavior.NoAction);

            // builder
            //     .Entity<UsersCommunities>()
            //     .HasOne(uc => uc.Community)
            //     .WithMany()
            //     .OnDelete(DeleteBehavior.NoAction);

            // builder.Entity<Question>()
            //     .Property(b => b.Created)
            //     .HasDefaultValueSql("getdate()");
            builder.Entity<Tag>().HasData(SeedTags());
        }

        //     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // => optionsBuilder
        //     .LogTo(Console.WriteLine)
        //     .EnableDetailedErrors();

        
        // protected override void OnConfiguring(DbContextOptionsBuilder options)
        // {
        //     // connect to sql server with connection string from app settings
        //     options
        //         .UseSqlServer(Configuration.GetConnectionString("vidcallme"));
        // }
        private static List<Tag> SeedTags()
        {
            return new List<Tag> {
                new Tag {
                    Id = 1,
                    Text = "JavaScript",
                    IsApproved = true
                    //Creator = null,
                },
                new Tag {
                    Id = 2,
                    Text = "Python",
                    IsApproved = true
                    //   Creator = null,
                },
                new Tag {
                    Id = 3,
                    Text = "React",
                    IsApproved = true
                    //  Creator = null,
                },
                new Tag {
                    Id = 4,
                    Text = "Angular",
                    IsApproved = true
                    //   Creator = null,
                }
            };
        }
    }
}
