using Microsoft.EntityFrameworkCore;
using DAL.Entities;

namespace DAL
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions options):base(options)
        {

        }

        public DbSet<AppUser> Users{get;set;}

        public DbSet<Project> Projects{get;set;}
    }

    
}