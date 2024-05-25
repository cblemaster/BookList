namespace BookList.API.Entities;

public partial class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Subtitle { get; set; }

    public bool IsFavorite { get; set; }

    public int GenreId { get; set; }

    public string? Publisher { get; set; }

    public int? PageCount { get; set; }

    public string? Description { get; set; }

    public virtual Genre Genre { get; set; } = null!;

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();
}
