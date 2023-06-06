using HH.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HH.Dao
{
    
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<Order> Orders { get; set; }

        public DbSet<Goods> Goods { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 定义数据模型
            modelBuilder.Entity<Order>()
                .HasKey(c => c.OrderId);

            modelBuilder.Entity<Goods>()
           .HasKey(c => c.Id);
            modelBuilder.Entity<User>()
           .HasKey(c => c.Id);
            modelBuilder.Entity<OrderDetail>()
           .HasKey(c => c.Id);
        }
    
}

}
