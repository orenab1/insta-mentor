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
    }

    
}