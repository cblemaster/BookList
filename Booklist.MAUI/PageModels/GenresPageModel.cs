using BookList.Core.DTO;
using BookList.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Booklist.MAUI.PageModels;

public partial class GenresPageModel(IDataService dataService) : ObservableObject
{
    private readonly IDataService _dataService = dataService;

    [ObservableProperty]
    private ObservableCollection<GenreDTO> _genres = default!;

    [ObservableProperty]
    private GenreDTO _selectedGenre = default!;

    [ObservableProperty]
    private bool _isGenreSelected;

    [RelayCommand]
    private async Task PageAppearingAsync()
    {
        await LoadDataAsync();
        SelectedGenre = null!;
        IsGenreSelected = false;
    }

    [RelayCommand]
    private void GenreSelected()
    {
        IsGenreSelected = SelectedGenre is not null;
    }

    private async Task LoadDataAsync() =>
        Genres = new ObservableCollection<GenreDTO>
            (await _dataService.GetGenresAsync());
}
