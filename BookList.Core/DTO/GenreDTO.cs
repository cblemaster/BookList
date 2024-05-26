namespace BookList.Core.DTO;

public record GenreDTO(int Id, string Name, bool IsFavorite)
{
    public static GenreDTO NotFound = new(0, "NotFound", false);
}
