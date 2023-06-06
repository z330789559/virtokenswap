using HH.Entities;
using Microsoft.EntityFrameworkCore;

namespace HH.Dao
{
    public class VirtualTokenDbContext:DbContext
    {
        public VirtualTokenDbContext(DbContextOptions<VirtualTokenDbContext> options)
            : base(options)
        {
           
        }

        public DbSet<VirtualToken> VirtualTokens { get; set; }

        public DbSet<Questionnaire> Questionnaires { get; set;}

        public DbSet<Question> Questions { get; set; }
        
        public DbSet<Answer> Answers { get; set; }

        public DbSet<Result> Results { get; set; }
    }
}
