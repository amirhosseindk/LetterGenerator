using System.Net.Http;
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
        public static void ConfigureLetterService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LetterDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ILetterRepository, LetterRepository>();

            var adapterBaseUrl = configuration.GetConnectionString("AdapterConnection")
                                 ?? throw new InvalidOperationException("AdapterConnection string is not set.");

            services.AddHttpClient<ILetterSyncAdapter, LetterSyncAdapter>(client =>
            {
                client.BaseAddress = new Uri(adapterBaseUrl);
            });

            services.AddScoped<ILetterService, LetterService>();
        }
    }
}