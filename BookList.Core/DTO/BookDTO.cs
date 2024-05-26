namespace BookList.Core.DTO;

public record BookDTO(int Id, string Title, string? Subtitle, bool IsFavorite,
    string? Publisher, int? PageCount, string? Description, GenreDTO Genre,
    IEnumerable<AuthorDTO> Authors);