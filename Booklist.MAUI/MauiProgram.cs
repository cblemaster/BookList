using Booklist.MAUI.PageModels;
using Booklist.MAUI.Pages;
using BookList.Core.Services;
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
                .Services.AddSingleton<IDataService, HttpDataService>()
                         .AddSingleton<AppShell>()
                         .AddTransient<BooksPage>()
                         .AddTransient<GenresPage>()
                         .AddTransient<AuthorsPage>()
                         .AddTransient<BooksPageModel>()
                         .AddTransient<GenresPageModel>()
                         .AddTransient<AuthorsPageModel>()
                         .AddTransient<CreateUpdateAuthorPageModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
