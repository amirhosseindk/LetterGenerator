using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LetterGenerator.Letter.Contracts
{
    public class LetterDbContextFactory : IDesignTimeDbContextFactory<LetterDbContext>
    {
        public LetterDbContext CreateDbContext(string[] args)
        {
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            SQLitePCL.Batteries.Init();

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("LettersProjectConfigs.json")
                .AddJsonFile("LettersProjectConfigs.Development.json", optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<LetterDbContext>();
            optionsBuilder.UseSqlite(config.GetConnectionString("DefaultConnection"));

            return new LetterDbContext(optionsBuilder.Options);
        }
    }
}