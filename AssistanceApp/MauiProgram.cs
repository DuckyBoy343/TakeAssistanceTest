using AssistanceApp.ViewModels;
using AssistanceApp.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace AssistanceApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<RestService>();
            builder.Services.AddSingleton<LocalDatabaseService>();
            builder.Services.AddSingleton<SyncService>();

            builder.Services.AddTransient<SchoolsViewModel>();

            builder.Services.AddTransient<SchoolsPage>();
            builder.Services.AddTransient<AttendanceViewModel>();
            builder.Services.AddTransient<AttendancePage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
