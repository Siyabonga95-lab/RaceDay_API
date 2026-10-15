using Microsoft.EntityFrameworkCore;
using RaceDay_API.Models;

namespace RaceDay_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for all your tables
        public DbSet<User> Users { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Organiser> Organisers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<EventEnrollment> EventEnrollments { get; set; }
        public DbSet<Result> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Unique constraints from your SQL script
            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique();
            modelBuilder.Entity<Participant>().HasIndex(p => p.ParticipantEmail).IsUnique();
            modelBuilder.Entity<Participant>().HasIndex(p => p.ParticipantPhone).IsUnique();
            modelBuilder.Entity<Organiser>().HasIndex(o => o.OrganiserEmail).IsUnique();
            modelBuilder.Entity<Organiser>().HasIndex(o => o.OrganiserPhone).IsUnique();
            modelBuilder.Entity<Result>().HasIndex(r => r.EnrollmentID).IsUnique();

            // Check constraints (using ToTable with check constraints is complex; we enforce in code too)
            base.OnModelCreating(modelBuilder);
        }
    }
}