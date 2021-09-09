using Microsoft.EntityFrameworkCore;
using RyhmatyoBuuttiServer.Models;

namespace RyhmatyoBuuttiServer
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Game>().ToTable("Game");
            modelBuilder.Entity<Publisher>().ToTable("Publisher");
            modelBuilder.Entity<Developer>().ToTable("Developer");
            modelBuilder.Entity<Genre>().ToTable("Genre");
        }

    }
}
