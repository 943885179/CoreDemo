using CoreDemo_Sql.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo_Sql.Db
{
    public class MyDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => base.OnConfiguring(optionsBuilder);

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            //modelBuilder.Entity<User>(o => o.Property(t => t.Name).IsRequired());
            base.OnModelCreating(modelBuilder);
        }
    }
}
