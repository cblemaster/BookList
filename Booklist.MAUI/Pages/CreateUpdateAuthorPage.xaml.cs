using Booklist.MAUI.PageModels;
using BookList.Core.DTO;

namespace Booklist.MAUI.Pages;

public partial class CreateUpdateAuthorPage : ContentPage
{
	public CreateUpdateAuthorPage(CreateUpdateAuthorDTO author)
	{
		InitializeComponent();

        Shell shell = Shell.Current;

        IViewHandler? handler = shell.Handler;
        if (handler is null) { return; }

        IMauiContext? context = handler.MauiContext;
        if (context is null) { return; }

        IServiceProvider services = context.Services;

        CreateUpdateAuthorPageModel pageModel = services.GetService<CreateUpdateAuthorPageModel>();
        if (pageModel is null) { return; }

        BindingContext = pageModel;
        pageModel.Author = author;
    }
}