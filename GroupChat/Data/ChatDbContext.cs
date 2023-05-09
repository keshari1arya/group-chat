
using GroupChat.Models;
using Microsoft.EntityFrameworkCore;


namespace GroupChat.Core;

public class ChatDbContext : DbContext
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
    {
       // Database.EnsureDeleted();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupMessage> GroupMessages { get; set; }
    public DbSet<MessageLike> MessageLikes { get; set; }
    public DbSet<GroupUserXREF> GroupUserXREF { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    { 
        modelBuilder.Entity<GroupUserXREF>()
           .HasKey(gu => new { gu.GroupId, gu.UserId });

        modelBuilder.Entity<GroupUserXREF>()
            .HasOne(gu => gu.Group)
            .WithMany(g => g.GroupUserXREF)
            .HasForeignKey(gu => gu.GroupId);

        modelBuilder.Entity<GroupUserXREF>()
            .HasOne(gu => gu.User)
            .WithMany(u => u.GroupUserXREF)
            .HasForeignKey(gu => gu.UserId);

        // Configure one-to-many relationship between Group and GroupMessage
        modelBuilder.Entity<Group>()
            .HasMany(g => g.GroupMessages)
            .WithOne(gm => gm.Group)
            .HasForeignKey(gm => gm.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure one-to-many relationship between GroupMessage and MessageLike
        modelBuilder.Entity<GroupMessage>()
            .HasMany(gm => gm.MessageLikes)
            .WithOne(ml => ml.GroupMessage)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure one-to-many relationship between GroupMessage and User (Sender)
        modelBuilder.Entity<GroupMessage>()
            .HasOne(gm => gm.Sender)
            .WithMany(u => u.GroupMessages)
            .HasForeignKey(gm => gm.SenderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure one-to-many relationship between MessageLike and User
        modelBuilder.Entity<MessageLike>()
            .HasOne(ml => ml.User)
            .WithMany(u => u.MessageLikes)
            .HasForeignKey(ml => ml.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);

        DbInitializer.Initialize(modelBuilder);
    }
}
