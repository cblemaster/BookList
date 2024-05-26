using Booklist.MAUI.PageModels;

namespace Booklist.MAUI.Pages;

public partial class BooksPage : ContentPage
{
    public BooksPage(BooksPageModel pageModel)
    {
        InitializeComponent();
        BindingContext = pageModel;
    }
}