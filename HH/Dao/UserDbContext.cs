using HH.Entities;
using Microsoft.EntityFrameworkCore;

namespace HH.Dao
{


    public class UserDbContext : DbContext
    {

        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }



    }
}
