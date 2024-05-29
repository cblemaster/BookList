using BookList.Core.DTO;
using BookList.Core.Entities;
using BookList.Core.Services;
using BookList.Core.Validation;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Booklist.MAUI.PageModels;

public partial class CreateUpdateBookPageModel : ObservableObject
{
    private readonly IDataService _dataService = default!;

    public CreateUpdateBookPageModel(IDataService dataService)
    {
        _dataService = dataService;
        Task.Run(() => LoadGenresAsync());
        Task.Run(() => LoadAuthorsAsync());
    }

    [ObservableProperty]
    private CreateUpdateBookDTO _book = default!;

    [ObservableProperty]
    private ObservableCollection<GenreDTO> _genres = default!;

    [ObservableProperty]
    private GenreDTO _selectedGenre = default!;

    [ObservableProperty]
    private ObservableCollection<AuthorDTO> _authors = default!;

    [ObservableProperty]
    private ObservableCollection<object> _selectedAuthors = default!;

    [RelayCommand]
    private void GenresLoaded()
    {
        if (Genres is not null && Book is not null)
        {
            SelectedGenre = Book.Genre;
        }
    }

    [RelayCommand]
    private void AuthorsLoaded()
    {
        if (Authors is not null && Book is not null)
        {
            SelectedAuthors = new(Book.Authors);
        }
    }

    [RelayCommand]
    private async Task SaveClicked()
    {
        ValidationResult validation = Book.Validate();

        if (!validation.IsValid)
        {
            await Shell.Current.DisplayAlert("Invalid Data!", validation.ErrorMessage, "OK");
            return;
        }

        if (SelectedGenre is null)
        {
            await Shell.Current.DisplayAlert("Invalid Data!", "Genre is required.", "OK");
            return;
        }

        Book.Genre = SelectedGenre;
        Book.Authors = SelectedAuthors.Cast<AuthorDTO>().ToList();

        if (Book.Id == 0)  //create
        {
            await _dataService.CreateBookAsync(Book);
        }
        else  //update
        {
            await _dataService.UpdateBookAsync(Book.Id, Book);
        }

        await Shell.Current.Navigation.PopModalAsync();
    }

    [RelayCommand]
    private async Task CancelClicked() =>
        await Shell.Current.Navigation.PopModalAsync();

    private async Task LoadGenresAsync()
    {
        Genres = new(await _dataService.GetGenresAsync());
    }

    private async Task LoadAuthorsAsync()
    {
        Authors = new(await _dataService.GetAuthorsAsync());
    }
}
