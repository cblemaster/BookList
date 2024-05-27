using BookList.Core.DTO;
using BookList.Core.Entities;
using BookList.Core.Services;
using BookList.Core.Validation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Booklist.MAUI.PageModels;

public partial class CreateUpdateGenrePageModel(IDataService dataService) : ObservableObject
{
    private readonly IDataService _dataService = dataService;

    [ObservableProperty]
    private CreateUpdateGenreDTO _genre = default!;

    [RelayCommand]
    private async Task SaveClicked()
    {
        string originalGenreName = Genre.Name;
        
        ValidationResult validation = Genre.Validate();

        if (!validation.IsValid)
        {
            await Shell.Current.DisplayAlert("Invalid Data!", validation.ErrorMessage, "OK");
            return;
        }

        if (Genre.Id == 0)  //create
        {
            if (await _dataService.IsGenreNameAlreadyUsed(Genre.Name))
            {
                await Shell.Current.DisplayAlert("Error!", $"Genre name {Genre.Name} is already used.", "OK");
                return;
            }
            await _dataService.CreateGenreAsync(Genre);
        }
        else  //update
        {
            if (Genre.Name != originalGenreName && await _dataService.IsGenreNameAlreadyUsed(Genre.Name))
            {
                await Shell.Current.DisplayAlert("Error!", $"Genre name {Genre.Name} is already used.", "OK");
                return;
            }
            await _dataService.UpdateGenreAsync(Genre.Id, Genre);
        }
        
        await Shell.Current.Navigation.PopModalAsync();
    }

    [RelayCommand]
    private async Task CancelClicked() =>
        await Shell.Current.Navigation.PopModalAsync();
}
