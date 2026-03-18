using Microsoft.EntityFrameworkCore;
using Crime.Models;

namespace Crime.Data
{
    public class CrimeDbContext : DbContext
    {
        public CrimeDbContext(DbContextOptions<CrimeDbContext> options)
            : base(options)
        {
        }

        public DbSet<Victim> Victims { get; set; } = null!;
        public DbSet<Suspect> Suspects { get; set; } = null!;
        public DbSet<Agency> Agencies { get; set; } = null!;
        public DbSet<Officer> Officers { get; set; } = null!;
        public DbSet<Incident> Incidents { get; set; } = null!;
        public DbSet<Evidence> Evidences { get; set; } = null!;
        public DbSet<Report> Reports { get; set; } = null!;
        public DbSet<Case> Cases { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.ReportingOfficer)
                .WithMany(o => o.Reports)
                .HasForeignKey(r => r.ReportingOfficerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Incident)
                .WithMany(i => i.Reports)
                .HasForeignKey(r => r.IncidentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}