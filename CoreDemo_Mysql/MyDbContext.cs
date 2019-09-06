using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo_Mysql
{
    public class MyDbContext:DbContext
    {
        public DbSet<user> user { get; set; }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
    }
    public class user
    {
        [Key]
        public string a { get; set; }
    }
}
