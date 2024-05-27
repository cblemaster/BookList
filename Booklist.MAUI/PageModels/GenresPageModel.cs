using Booklist.MAUI.Pages;
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
    private void GenreSelected() => IsGenreSelected = SelectedGenre is not null;

    [RelayCommand]
    private async Task CreateGenreAsync() =>
        await Shell.Current.Navigation
            .PushModalAsync(new CreateUpdateGenrePage(new()));

    [RelayCommand]
    private async Task DeleteSelectedGenreAsync()
    {
        bool deleteConfirmed = await Shell.Current.CurrentPage.DisplayAlert("Delete?", $"Are you sure you want to delete genre {SelectedGenre.Name}?", "Yes, delete", "No, cancel");

        if (!deleteConfirmed) { return; }

        if (await _dataService.DoesGenreHaveBooks(SelectedGenre.Id))
        {
            await Shell.Current.DisplayAlert("Error!", "Cannot delete genre since it is associated with one or more books.", "OK");
            return;
        }

        await _dataService.DeleteGenreAsync(SelectedGenre.Id);
        await LoadDataAsync();
        SelectedGenre = null!;
    }

    [RelayCommand]
    private async Task UpdateGenreAsync() =>
        await Shell.Current.Navigation
            .PushModalAsync(new CreateUpdateGenrePage
                (new()
                {
                    Id = SelectedGenre.Id,
                    Name = SelectedGenre.Name,
                    IsFavorite = SelectedGenre.IsFavorite
                }));

    private async Task LoadDataAsync() =>
        Genres = new ObservableCollection<GenreDTO>
            (await _dataService.GetGenresAsync());
}
