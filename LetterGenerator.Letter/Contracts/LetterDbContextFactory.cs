using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LetterGenerator.Letter.Contracts
{
    public class LetterDbContextFactory : IDesignTimeDbContextFactory<LetterDbContext>
    {
        public LetterDbContext CreateDbContext(string[] args)
        {
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            SQLitePCL.Batteries.Init();

            var optionsBuilder = new DbContextOptionsBuilder<LetterDbContext>();

            optionsBuilder.UseSqlite("Data Source=LetterDb.db");

            return new LetterDbContext(optionsBuilder.Options);
        }
    }
}