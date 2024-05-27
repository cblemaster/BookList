using BookList.Core.Validation;

namespace BookList.Core.DTO;

public class CreateUpdateGenreDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsFavorite { get; set; }

    public ValidationResult Validate() =>
        !string.IsNullOrWhiteSpace(Name) && Name.Length > 0 && Name.Length <= 100
            ? new() { IsValid = true, ErrorMessage = string.Empty }
            : new()
            {
                IsValid = false,
                ErrorMessage =
                "Genre name is required and must be between 1 and 100 characters."
            };
}
