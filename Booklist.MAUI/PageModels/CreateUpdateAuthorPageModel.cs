using BookList.Core.DTO;
using BookList.Core.Services;
using BookList.Core.Validation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Booklist.MAUI.PageModels;

public partial class CreateUpdateAuthorPageModel(IDataService dataService) : ObservableObject
{
    private readonly IDataService _dataService = dataService;

    [ObservableProperty]
    private CreateUpdateAuthorDTO _author = default!;

    [RelayCommand]
    private async Task SaveClicked()
    {
        ValidationResult validation = Author.Validate();

        if (!validation.IsValid)
        {
            await Shell.Current.DisplayAlert("Invalid Data!", validation.ErrorMessage, "OK");
            return;
        }

        if (await _dataService.IsAuthorNameAlreadyUsed(Author.Name))
        {
            await Shell.Current.DisplayAlert("Error!", $"Author name { Author.Name } is already used.", "OK");
            return;
        }

        if (Author.Id == 0)  //create
        {
            await _dataService.CreateAuthorAsync(Author);
        }
        else  //update
        {
            await _dataService.UpdateAuthorAsync(Author.Id, Author);
        }
        
        await Shell.Current.Navigation.PopModalAsync();
    }

    [RelayCommand]
    private async Task CancelClicked() =>
        await Shell.Current.Navigation.PopModalAsync();
}
