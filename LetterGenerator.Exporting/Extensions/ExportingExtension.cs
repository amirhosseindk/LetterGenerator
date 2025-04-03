using LetterGenerator.Exporting.Contracts;
using LetterGenerator.Exporting.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LetterGenerator.Exporting.Extensions
{
    public static class ExportingExtension
    {
        public static void ConfigureExportingServices(this IServiceCollection services, string templateDirectory)
        {
            services.AddScoped<IDocumentGenerationService>(sp =>
                new DocumentGenerationService(templateDirectory));
        }
    }
}