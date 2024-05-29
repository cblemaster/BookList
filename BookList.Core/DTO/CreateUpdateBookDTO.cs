using BookList.Core.Entities;

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
}
