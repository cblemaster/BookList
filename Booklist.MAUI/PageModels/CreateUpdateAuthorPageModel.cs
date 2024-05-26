using BookList.Core.DTO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Booklist.MAUI.PageModels;

public partial class CreateUpdateAuthorPageModel : ObservableObject
{
    [ObservableProperty]
    private CreateUpdateAuthorDTO _author = default!;
}
