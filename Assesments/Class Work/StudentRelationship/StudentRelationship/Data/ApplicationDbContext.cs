using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentRelationship.Models;

namespace StudentRelationship.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<StudentStream> Streams { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }
}
