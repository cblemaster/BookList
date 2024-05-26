using BookList.Core.DTO;
using BookList.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Booklist.MAUI.PageModels;

public partial class BooksPageModel(IDataService dataService) : ObservableObject
{
    private readonly IDataService _dataService = dataService;

    [ObservableProperty]
    private ObservableCollection<BookDTO> _books = default!;

    [RelayCommand]
    private async Task PageAppearingAsync() => await LoadDataAsync();

    private async Task LoadDataAsync() =>
        Books = new ObservableCollection<BookDTO>
            (await _dataService.GetBooksAsync());
}
