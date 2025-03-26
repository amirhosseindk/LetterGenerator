using Microsoft.EntityFrameworkCore;

namespace LetterGenerator.Letter.Contracts
{
    public class LetterDbContext : DbContext
    {
        public LetterDbContext(DbContextOptions<LetterDbContext> options) : base(options) { }

        public DbSet<Models.Letter> Letters { get; set; }
    }
}