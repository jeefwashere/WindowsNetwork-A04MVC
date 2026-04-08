using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Models;

namespace UserManagementSystem.Data
{
    // Found here: https://learn.microsoft.com/en-us/aspnet/core/data/ef-rp/intro?view=aspnetcore-10.0&tabs=visual-studio
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<UserItem> UserItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserItem>()
                .HasOne(i => i.Owner)
                .WithMany(u => u.Items)
                .HasForeignKey(i => i.OwnerID);
        }
    }
}
