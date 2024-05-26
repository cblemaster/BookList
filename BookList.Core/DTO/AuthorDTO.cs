namespace BookList.Core.DTO;

public record AuthorDTO(int Id, string Name, bool IsFavorite)
{
    public static AuthorDTO NotFound = new(0, "NotFound", false);
}