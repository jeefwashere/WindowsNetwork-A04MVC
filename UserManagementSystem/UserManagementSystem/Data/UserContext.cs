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

        //have a user table in the data base
        public DbSet<User> User { get; set; }
        // have a user item table for data base
        public DbSet<UserItem> UserItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //set up useritem entity
            modelBuilder.Entity<UserItem>()
                .HasOne(i => i.Owner)//one useritem have one owener
                .WithMany(u => u.Items)//one user have many useritems
                .HasForeignKey(i => i.OwnerID);//the foreign key in useritem is ownerid
        }
    }
}
