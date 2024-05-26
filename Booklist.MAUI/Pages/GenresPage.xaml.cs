using Booklist.MAUI.PageModels;

namespace Booklist.MAUI.Pages;

public partial class GenresPage : ContentPage
{
    public GenresPage(GenresPageModel pageModel)
    {
        InitializeComponent();
        BindingContext = pageModel;
    }
}