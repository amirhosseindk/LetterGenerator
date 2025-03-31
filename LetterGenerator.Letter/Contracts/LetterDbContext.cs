using LetterGenerator.Letter.Models;
using Microsoft.EntityFrameworkCore;

namespace LetterGenerator.Letter.Contracts
{
    public class LetterDbContext : DbContext
    {
        public LetterDbContext(DbContextOptions<LetterDbContext> options) : base(options) { }

        public DbSet<Models.Letter> Letters { get; set; }
        public DbSet<LetterSyncStatus> LetterSyncStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LetterSyncStatus>()
                .HasKey(l => new { l.LetterId, l.Username, l.DeviceType });

            modelBuilder.Entity<LetterSyncStatus>()
                .HasOne(l => l.Letter)
                .WithMany()
                .HasForeignKey(l => l.LetterId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}