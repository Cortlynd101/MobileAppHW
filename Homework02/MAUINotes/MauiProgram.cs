using MAUINotes.Initializers;
using MAUINotes.Services;
using Microsoft.Extensions.Logging;
using NotesLib.Data;
using NotesLib.Services;


namespace MAUINotes
{
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
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddScoped<IUserService, MauiUserService>();
            builder.Services.AddSingleton<LocalDatabaseInitializer>();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
