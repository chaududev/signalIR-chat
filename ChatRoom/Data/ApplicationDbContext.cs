using ChatRoom.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatRoom.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var hasher = new PasswordHasher<IdentityUser>();

            modelBuilder.Entity<Message>().ToTable("Messages")
                .Property(s => s.Content).IsRequired().HasMaxLength(500);
            modelBuilder.Entity<Room>().ToTable("Rooms")
                .Property(s => s.Name).IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Message>()
                .HasOne(s => s.ToRoom)
                .WithMany(m => m.Messages)
                .HasForeignKey(s => s.ToRoomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Room>().HasOne(s => s.Admin)
                .WithMany(u => u.Rooms)
                .IsRequired();
            modelBuilder.Entity<User>().HasData(
                new
                {
                    Id = "68fefa20-a19d-445f-affc-89f984d555b8",
                    UserName = "chaudu",
                    NormalizedUserName = "CHAUDU",
                    Email = "chaudu301@gmail.com",
                    NormalizedEmail = "CHAUDU301@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Abcd@123"),
                    SecurityStamp = "AKMZLDVQDMJAX4AKBITZL5OOVZB6SHPN",
                    ConcurrencyStamp = "e26d8cb5-e3ce-4e0c-9588-ffd39ee998b1",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    FullName = "Chau Du"
                }
            );
        }
    }
}