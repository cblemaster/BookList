using BookList.Core.Entities;
using BookList.Core.Validation;
using System.Xml.Linq;

namespace BookList.Core.DTO;

public class CreateUpdateBookDTO
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Subtitle { get; set; }

    public bool IsFavorite { get; set; }

    public string? Publisher { get; set; }

    public int? PageCount { get; set; }

    public string? Description { get; set; }

    public virtual GenreDTO Genre { get; set; } = null!;

    public virtual ICollection<AuthorDTO> Authors { get; set; } = new List<AuthorDTO>();

    public ValidationResult Validate()
    {
        bool titleIsValid = !string.IsNullOrWhiteSpace(Title) 
            && Title.Length > 0
            && Title.Length <= 100;
        if (!titleIsValid)
        {
            return new() { IsValid = false, ErrorMessage = "Title is required and must be between 1 and 100 characters." };
        }

        bool subtitleIsValid = Subtitle is null 
            || (!string.IsNullOrWhiteSpace(Subtitle)
                && Subtitle.Length > 0
                && Subtitle.Length <= 100);
        if (!subtitleIsValid)
        {
            return new() { IsValid = false, ErrorMessage = "Subtitle, if provided, must be between 1 and 100 characters." };
        }

        bool publisherIsValid = Publisher is null
            || (!string.IsNullOrWhiteSpace(Publisher)
                && Publisher.Length > 0
                && Publisher.Length <= 100);
        if (!publisherIsValid)
        {
            return new() { IsValid = false, ErrorMessage = "Publisher, if provided, must be between 1 and 100 characters." };
        }

        bool descriptionIsValid = Description is null
            || (!string.IsNullOrWhiteSpace(Description)
                && Description.Length > 0
                && Description.Length <= 255);
        if (!descriptionIsValid)
        {
            return new() { IsValid = false, ErrorMessage = "Description, if provided, must be between 1 and 255 characters." };
        }

        return new() { IsValid = true, ErrorMessage = string.Empty };
    }
}
