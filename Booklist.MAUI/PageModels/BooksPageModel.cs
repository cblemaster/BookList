using Booklist.MAUI.Pages;
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

    [ObservableProperty]
    private BookDTO _selectedBook = default!;

    [ObservableProperty]
    private bool _isBookSelected;

    [RelayCommand]
    private async Task PageAppearingAsync()
    {
        await LoadDataAsync();
        SelectedBook = null!;
        IsBookSelected = false;
    }

    [RelayCommand]
    private void BookSelected() => IsBookSelected = SelectedBook is not null;

    [RelayCommand]
    private async Task CreateBookAsync() =>
        await Shell.Current.Navigation
            .PushModalAsync(new CreateUpdateBookPage(new()));

    [RelayCommand]
    private async Task DeleteSelectedBookAsync()
    {
        bool deleteConfirmed = await Shell.Current.CurrentPage.DisplayAlert("Delete?", $"Are you sure you want to delete book {SelectedBook.Title}?", "Yes, delete", "No, cancel");

        if (!deleteConfirmed) { return; }

        await _dataService.DeleteBookAsync(SelectedBook.Id);
        await LoadDataAsync();
        SelectedBook = null!;
    }

    [RelayCommand]
    private async Task UpdateSelectedBookAsync() =>
        await Shell.Current.Navigation
            .PushModalAsync(new CreateUpdateBookPage
                (new()
                {
                    Id = SelectedBook.Id,
                    Title = SelectedBook.Title,
                    Subtitle = SelectedBook.Subtitle,
                    IsFavorite = SelectedBook.IsFavorite,
                    Publisher = SelectedBook.Publisher,
                    PageCount = SelectedBook.PageCount,
                    Description = SelectedBook.Description,
                    Genre = SelectedBook.Genre,
                    Authors = SelectedBook.Authors.ToList(),
                }));

    private async Task LoadDataAsync() =>
        Books = new ObservableCollection<BookDTO>
            (await _dataService.GetBooksAsync());
}
