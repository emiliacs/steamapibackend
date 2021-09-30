using Microsoft.EntityFrameworkCore;
using RyhmatyoBuuttiServer.Models;

namespace RyhmatyoBuuttiServer
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<UserGame> UserGames { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Game>().ToTable("Game");
            modelBuilder.Entity<Publisher>().ToTable("Publisher");
            modelBuilder.Entity<Developer>().ToTable("Developer");
            modelBuilder.Entity<Genre>().ToTable("Genre");
            modelBuilder.Entity<UserGame>().ToTable("UserGame");
        }

    }
}
