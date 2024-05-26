using Booklist.MAUI.Pages;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace Booklist.MAUI
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
                })
                .Services.AddSingleton<AppShell>()
                         .AddSingleton<BooksPage>()
                         .AddSingleton<GenresPage>()
                         .AddSingleton<AuthorsPage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
