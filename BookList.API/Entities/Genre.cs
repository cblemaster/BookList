namespace BookList.API.Entities;

public partial class Genre
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsFavorite { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
