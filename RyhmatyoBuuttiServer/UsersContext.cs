using Microsoft.EntityFrameworkCore;
using RyhmatyoBuuttiServer.Models;

namespace RyhmatyoBuuttiServer
{
    public class UsersContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public UsersContext(DbContextOptions<UsersContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
        }

    }
}
