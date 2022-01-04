using Microsoft.EntityFrameworkCore;
using DAL.Entities;

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
        }
    }

    
}