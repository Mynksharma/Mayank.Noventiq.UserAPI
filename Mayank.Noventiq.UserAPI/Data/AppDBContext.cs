using Mayank.Noventiq.UserAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mayank.Noventiq.UserAPI.Data
{
    public class AppDBContext:DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options):base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)           // Each User has one Role
                .WithMany(r => r.Users)        // Each Role has many Users
                .HasForeignKey(u => u.RoleId)  // Foreign key in User table
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

        }
    }
}
