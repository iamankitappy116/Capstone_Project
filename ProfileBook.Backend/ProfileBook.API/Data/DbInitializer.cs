using System.Security.Cryptography;
using System.Text;
using ProfileBook.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ProfileBook.API.Data;

public static class DbInitializer
{
    public static async Task Seed(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();

        // Always ensure admin exists. If not, re-seed.
        if (await context.Users.AnyAsync(u => u.Username == "admin"))
        {
            Console.WriteLine("[SEED] Admin user already exists. Skipping seed.");
            return;
        }

        Console.WriteLine("[SEED] Seeding database...");
        // Clear existing data to ensure a fresh start for debugging
        context.Reports.RemoveRange(context.Reports);
        context.Likes.RemoveRange(context.Likes);
        context.Comments.RemoveRange(context.Comments);
        context.GroupMembers.RemoveRange(context.GroupMembers);
        context.Posts.RemoveRange(context.Posts);
        context.Groups.RemoveRange(context.Groups);
        context.Users.RemoveRange(context.Users);
        await context.SaveChangesAsync();

        CreatePasswordHash("password123", out byte[] hash, out byte[] salt);

        // 1. Seed 15 Users
        var users = new List<User>();
        users.Add(new User { Username = "admin", Email = "admin@profilebook.com", PasswordHash = hash, PasswordSalt = salt, Role = "Admin", Bio = "System Administrator", Location = "Global", CreatedAt = DateTime.UtcNow.AddDays(-100) });
        
        for (int i = 1; i <= 14; i++)
        {
            users.Add(new User 
            { 
                Username = $"user{i}", 
                Email = $"user{i}@example.com", 
                PasswordHash = hash, 
                PasswordSalt = salt, 
                Role = "User", 
                Bio = $"Developer and enthusiast #{i}", 
                Location = i % 2 == 0 ? "New York" : "London", 
                CreatedAt = DateTime.UtcNow.AddDays(-i * 5) 
            });
        }
        context.Users.AddRange(users);
        await context.SaveChangesAsync();

        // 2. Seed 12 Groups
        var groupNames = new[] { "Tech Talk", "Gamers United", "Foodies Hub", "Travel World", "Art Studio", "Fitness Club", "Coding Camp", "Music Beats", "Nature Seekers", "Movie Buffs", "Science Daily", "History Nerds" };
        var categories = new[] { "Technology", "Gaming", "Food", "Travel", "Art", "Sports", "Technology", "Music", "Nature", "Entertainment", "Education", "Education" };
        var groups = new List<Group>();
        for (int i = 0; i < groupNames.Length; i++)
        {
            groups.Add(new Group
            {
                GroupName = groupNames[i],
                Category = categories[i],
                Description = $"Discussion forum for {groupNames[i]}.",
                CreatedByUserId = users[i % users.Count].UserId
            });
        }
        context.Groups.AddRange(groups);
        await context.SaveChangesAsync();

        // 3. Seed Group Members
        foreach (var group in groups)
        {
            var ownerId = group.CreatedByUserId;
            if (!context.GroupMembers.Any(gm => gm.GroupId == group.GroupId && gm.UserId == ownerId))
                context.GroupMembers.Add(new GroupMember { GroupId = group.GroupId, UserId = ownerId });

            // Add 3-5 random members to each group
            for (int k = 1; k <= 5; k++)
            {
                var randomUserId = users[(group.GroupId + k) % users.Count].UserId;
                if (!context.GroupMembers.Any(gm => gm.GroupId == group.GroupId && gm.UserId == randomUserId))
                {
                    context.GroupMembers.Add(new GroupMember { GroupId = group.GroupId, UserId = randomUserId });
                }
            }
        }
        await context.SaveChangesAsync();

        // 4. Seed 20 Posts (Mix of Global and Group Posts)
        var posts = new List<Post>();
        for (int i = 0; i < 20; i++)
        {
            posts.Add(new Post
            {
                Content = $"Knowledge post #{i + 1}: Sharing some interesting insights about {(i % 2 == 0 ? "the world" : "the community")}.",
                UserId = users[i % users.Count].UserId,
                Status = i < 15 ? "Approved" : "Pending",
                CreatedAt = DateTime.UtcNow.AddHours(-i * 6),
                GroupId = i < 10 ? groups[i % groups.Count].GroupId : null
            });
        }
        context.Posts.AddRange(posts);
        await context.SaveChangesAsync();

        // 5. Seed 25 Comments
        for (int i = 0; i < 25; i++)
        {
            context.Comments.Add(new Comment
            {
                CommentText = $"Very insightful! Keep it up. Reply #{i + 1}",
                UserId = users[(i + 3) % users.Count].UserId,
                PostId = posts[i % 15].PostId, // Comment on approved posts
                CreatedAt = DateTime.UtcNow.AddMinutes(-i * 45)
            });
        }

        // 6. Seed 30 Likes
        for (int i = 0; i < 30; i++)
        {
            context.Likes.Add(new Like
            {
                UserId = users[(i + 5) % users.Count].UserId,
                PostId = posts[i % 18].PostId
            });
        }

        // 7. Seed 15 Reports
        var reasons = new[] { "Inappropriate", "Spam", "Harassment", "Hate Speech", "Misinformation" };
        var severities = new[] { "Low", "Medium", "High", "Critical" };
        for (int i = 0; i < 15; i++)
        {
            context.Reports.Add(new Report
            {
                Reason = reasons[i % reasons.Length],
                ReportedUserId = users[(i + 1) % users.Count].UserId,
                ReportingUserId = users[(i + 10) % users.Count].UserId,
                TimeStamp = DateTime.UtcNow.AddHours(-i * 3)
            });
        }

        await context.SaveChangesAsync();
        Console.WriteLine("Database Seeding Completed Successfully.");
    }

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
}
