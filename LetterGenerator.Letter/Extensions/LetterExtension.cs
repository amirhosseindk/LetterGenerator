using LetterGenerator.Letter.Contracts;
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

            //services.AddScoped<ILetterService, LetterService>();
        }
    }
}