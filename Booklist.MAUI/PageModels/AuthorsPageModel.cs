using BookList.Core.DTO;
using BookList.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Booklist.MAUI.PageModels;

public partial class AuthorsPageModel(IDataService dataService) : ObservableObject
{
    private readonly IDataService _dataService = dataService;

    [ObservableProperty]
    private ObservableCollection<AuthorDTO> _authors = default!;

    [ObservableProperty]
    private AuthorDTO _selectedAuthor = default!;

    [ObservableProperty]
    private bool _isAuthorSelected;

    [RelayCommand]
    private async Task PageAppearingAsync()
    {
        await LoadDataAsync();
        SelectedAuthor = null!;
        IsAuthorSelected = false;
    }

    [RelayCommand]
    private void AuthorSelected() => IsAuthorSelected = SelectedAuthor is not null;

    [RelayCommand]
    private async Task DeleteSelectedAuthorAsync()
    {
        bool deleteConfirmed = await Shell.Current.CurrentPage.DisplayAlert("Delete?", $"Are you sure you want to delete author { SelectedAuthor.Name }?", "Yes, delete", "No, cancel");

        if (!deleteConfirmed) { return; }

        if (await _dataService.DoesAuthorHaveBooks(SelectedAuthor.Id))
        {
            await Shell.Current.DisplayAlert("Error!", "Cannot delete author since it is associated with one or more books.", "OK");
            return;
        }

        await _dataService.DeleteAuthorAsync(SelectedAuthor.Id);
        await LoadDataAsync();
        SelectedAuthor = null!;
    }

    private async Task LoadDataAsync() =>
        Authors = new ObservableCollection<AuthorDTO>
            (await _dataService.GetAuthorsAsync());
}
