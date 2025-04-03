using LetterGenerator.Letter.Extensions;
using Microsoft.Extensions.Logging;
using LetterGenerator.Letter.Contracts;
using LetterGenerator.Exporting.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LetterGenerator.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.ConfigureLetterService(builder.Configuration, Path.Combine(FileSystem.AppDataDirectory, "LetterDb.db"));
        builder.Services.ConfigureExportingServices(FileSystem.AppDataDirectory);


        #if DEBUG
        builder.Logging.AddDebug();
        #endif

        using (var scope = builder.Services.BuildServiceProvider().CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<LetterDbContext>();
            db.Database.Migrate();
        }

        return builder.Build();
    }
}