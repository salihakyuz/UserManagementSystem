using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAdress> Adresses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) //DİKKAT
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
