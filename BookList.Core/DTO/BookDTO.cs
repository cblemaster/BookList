namespace BookList.Core.DTO;

public record BookDTO(int Id, string Title, string? Subtitle, bool IsFavorite,
    string? Publisher, int? PageCount, string? Description, GenreDTO Genre,
    IEnumerable<AuthorDTO> Authors)
{
    public static BookDTO NotFound = new(0, "NotFound", string.Empty,
        false, null, null, null, GenreDTO.NotFound, 
        new List<AuthorDTO>() { AuthorDTO.NotFound } );
}