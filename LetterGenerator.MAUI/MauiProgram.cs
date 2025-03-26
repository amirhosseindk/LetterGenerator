using LetterGenerator.User.Extensions;
using LetterGenerator.Letter.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LetterGenerator.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.Configuration.AddJsonFile("UsersProjectConfigs.json", optional: false, reloadOnChange: true)
                             .AddJsonFile("UsersProjectConfigs.Development.json", optional: true, reloadOnChange: true);

        builder.Configuration.AddJsonFile("LettersProjectConfigs.json", optional: false, reloadOnChange: true)
                     .AddJsonFile("LettersProjectConfigs.Development.json", optional: true, reloadOnChange: true);

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.ConfigureUserService(builder.Configuration);
        builder.Services.ConfigureLetterService(builder.Configuration);

        #if DEBUG
        builder.Logging.AddDebug();
        #endif

        return builder.Build();
    }
}