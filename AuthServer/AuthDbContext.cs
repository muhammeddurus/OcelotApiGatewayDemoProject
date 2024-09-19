using AuthServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace AuthServer
{
    public class AuthDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AuthDbContext(DbContextOptions dbContextOptions):base(dbContextOptions)
        {
        }

    }
}
