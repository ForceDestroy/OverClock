using Microsoft.EntityFrameworkCore;
using Server.DbModels;
using System.Diagnostics.CodeAnalysis;

namespace Server.Data
{
    [ExcludeFromCodeCoverage]
    public class DatabaseContext:DbContext
    {
        private readonly string _connectionString;
        public DbSet<ModuleStatus> ModuleStatus { get; set; } = null!;
        public DbSet<WorkHours> WorkHours { get; set; } = null!;
        public DbSet<Announcement> Announcements { get; set; } = null!;
        public DbSet<PaySlip> Payslips { get; set; } = null!;
        public DbSet<Email> Emails { get; set; } = null!;
        public DbSet<Module> Modules { get; set; } = null!;
        public DbSet<Policy> Policies { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        public DbSet<JobPosting> JobPostings { get; set; } = null!;

        public DbSet<Application> Applications { get; set; } = null!;

        public DbSet<Request> Requests { get; set; } = null!;
        public DbSet<Schedule> Schedules { get; set; } = null!;

        public DbSet<SessionToken> SessionTokens { get; set; } = null!;

        public DatabaseContext(Dbconnection c)
        {
            _connectionString = c.ConnectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer($"{_connectionString}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Email>().HasOne(i => i.From).WithMany(u => u.SentEmails).HasForeignKey(i => i.FromId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Email>().HasOne(i => i.To).WithMany(u => u.ReceivedEmails).HasForeignKey(i => i.ToId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Request>().HasOne(i => i.From).WithMany(u => u.SentRequests).HasForeignKey(i => i.FromId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Request>().HasOne(i => i.To).WithMany(u => u.ReceivedRequests).HasForeignKey(i => i.ToId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>().HasOne(i => i.User).WithMany(u => u.Applications).HasForeignKey(i => i.UserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Application>().HasOne(i => i.Posting).WithMany(p => p.Applications).HasForeignKey(i => i.PostingId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
