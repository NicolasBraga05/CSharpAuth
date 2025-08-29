
using CSharpAuth.Models;
using Microsoft.EntityFrameworkCore;

namespace CSharpAuth.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Project { get; set; }
    }
}