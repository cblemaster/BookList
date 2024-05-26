using Booklist.MAUI.PageModels;

namespace Booklist.MAUI.Pages;

public partial class AuthorsPage : ContentPage
{
    public AuthorsPage(AuthorsPageModel pageModel)
    {
        InitializeComponent();
        BindingContext = pageModel;
    }
}