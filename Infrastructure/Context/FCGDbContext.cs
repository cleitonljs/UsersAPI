using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Context
{
    public class FCGDbContext : DbContext
    {
        public FCGDbContext(DbContextOptions<FCGDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FCGDbContext).Assembly);

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Nome = "Admin", Email = "admin@email.com", Senha = "$2a$12$LUcHRhocaRhvl3bM0MksHupKRvJF4JzG4Ku9nn/1kGYLQKTYJyR7u", Role = 1 }
            );

        }
    }
}
