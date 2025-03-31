using LetterGenerator.Letter.Adapters;
using LetterGenerator.Letter.Contracts;
using LetterGenerator.Letter.Repositories;
using LetterGenerator.Letter.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LetterGenerator.Letter.Extensions
{
    public static class LetterExtension
    {
        public static void ConfigureLetterService(this IServiceCollection services, IConfiguration configuration, string dbPath)
        {
            services.AddDbContext<LetterDbContext>(options =>
            {
                options.UseSqlite($"Filename={dbPath}");
            });

            services.AddScoped<ILetterRepository, LetterRepository>();

            var adapterBaseUrl = configuration.GetConnectionString("AdapterConnection") ?? "https://localhost:5001";

            services.AddHttpClient<ILetterSyncAdapter, LetterSyncAdapter>(client =>
            {
                client.BaseAddress = new Uri(adapterBaseUrl);
            });

            services.AddScoped<ILetterService, LetterService>();
        }
    }
}