using Booklist.MAUI.PageModels;
using BookList.Core.DTO;

namespace Booklist.MAUI.Pages;

public partial class CreateUpdateGenrePage : ContentPage
{
    public CreateUpdateGenrePage(CreateUpdateGenreDTO genre)
    {
        InitializeComponent();

        Shell shell = Shell.Current;

        IViewHandler? handler = shell.Handler;
        if (handler is null) { return; }

        IMauiContext? context = handler.MauiContext;
        if (context is null) { return; }

        IServiceProvider services = context.Services;

        CreateUpdateGenrePageModel pageModel = services.GetService<CreateUpdateGenrePageModel>();
        if (pageModel is null) { return; }

        BindingContext = pageModel;
        pageModel.Genre = genre;
    }
}