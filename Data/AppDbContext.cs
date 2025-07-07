using Microsoft.EntityFrameworkCore;
using Skinalyze.API.Models;

namespace Skinalyze.API.Data
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

    }

}

