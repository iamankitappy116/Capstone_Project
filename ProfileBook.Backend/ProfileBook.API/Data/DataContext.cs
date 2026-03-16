using Microsoft.EntityFrameworkCore;
using ProfileBook.API.Models;

namespace ProfileBook.API.Data;

public class DataContext : DbContext
{
  public DataContext(DbContextOptions<DataContext> options) : base(options) { }

  public DbSet<User> Users => Set<User>();
  public DbSet<Post> Posts => Set<Post>();
  public DbSet<Message> Messages => Set<Message>();
  public DbSet<Report> Reports => Set<Report>();
  public DbSet<Group> Groups => Set<Group>();
  public DbSet<Comment> Comments => Set<Comment>();
  public DbSet<Like> Likes => Set<Like>();
  public DbSet<GroupMember> GroupMembers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Message>()
        .HasOne(m => m.Sender)
        .WithMany()
        .HasForeignKey(m => m.SenderId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Message>()
        .HasOne(m => m.Receiver)
        .WithMany()
        .HasForeignKey(m => m.ReceiverId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Report>()
        .HasOne(r => r.ReportedUser)
        .WithMany()
        .HasForeignKey(r => r.ReportedUserId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Report>()
        .HasOne(r => r.ReportingUser)
        .WithMany()
        .HasForeignKey(r => r.ReportingUserId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Comment>()
        .HasOne(c => c.User)
        .WithMany(u => u.Comments)
        .HasForeignKey(c => c.UserId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Comment>()
        .HasOne(c => c.Post)
        .WithMany(p => p.Comments)
        .HasForeignKey(c => c.PostId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Like>()
        .HasOne(l => l.User)
        .WithMany(u => u.Likes)
        .HasForeignKey(l => l.UserId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Like>()
        .HasOne(l => l.Post)
        .WithMany(p => p.Likes)
        .HasForeignKey(l => l.PostId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<GroupMember>()
         .HasOne(gm => gm.User)
         .WithMany()
         .HasForeignKey(gm => gm.UserId)
         .OnDelete(DeleteBehavior.NoAction);

    modelBuilder.Entity<Group>()
        .HasOne(g => g.CreatedByUser)
        .WithMany()
        .HasForeignKey(g => g.CreatedByUserId)
        .OnDelete(DeleteBehavior.NoAction);

    modelBuilder.Entity<GroupMember>()
         .HasOne(gm => gm.Group)
         .WithMany(g => g.Members)
         .HasForeignKey(gm => gm.GroupId);
    }

}

