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

    [RelayCommand]
    private async Task PageAppearingAsync() => await LoadDataAsync();

    private async Task LoadDataAsync() =>
        Genres = new ObservableCollection<GenreDTO>
            (await _dataService.GetGenresAsync());
}
