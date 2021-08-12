using Microsoft.EntityFrameworkCore;
using RyhmatyoBuuttiServer.Models;

namespace RyhmatyoBuuttiServer
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<PasswordResetCode> ResetCodes { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<PasswordResetCode>().ToTable("ResetCode");
        }

    }
}
