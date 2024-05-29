using Booklist.MAUI.PageModels;
using BookList.Core.DTO;
using BookList.Core.Entities;

namespace Booklist.MAUI.Pages;

public partial class CreateUpdateBookPage : ContentPage
{
    public CreateUpdateBookPage(CreateUpdateBookDTO book)
    {
        InitializeComponent();

        Shell shell = Shell.Current;

        IViewHandler? handler = shell.Handler;
        if (handler is null) { return; }

        IMauiContext? context = handler.MauiContext;
        if (context is null) { return; }

        IServiceProvider services = context.Services;

        CreateUpdateBookPageModel pageModel = services.GetService<CreateUpdateBookPageModel>();
        if (pageModel is null) { return; }

        BindingContext = pageModel;
        pageModel.Book = book;
    }
}