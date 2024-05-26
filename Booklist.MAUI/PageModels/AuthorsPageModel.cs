using BookList.Core.DTO;
using BookList.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Booklist.MAUI.PageModels;

public partial class AuthorsPageModel(IDataService dataService) : ObservableObject
{
    private readonly IDataService _dataService = dataService;

    [ObservableProperty]
    private ObservableCollection<AuthorDTO> _authors = default!;

    [RelayCommand]
    private async Task PageAppearingAsync() => await LoadDataAsync();

    private async Task LoadDataAsync() =>
        Authors = new ObservableCollection<AuthorDTO>
            (await _dataService.GetAuthorsAsync());
}
