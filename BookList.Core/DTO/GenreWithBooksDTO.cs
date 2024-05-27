namespace BookList.Core.DTO;

public record GenreWithBooksDTO(int Id, string Name, bool IsFavorite, 
    IEnumerable<BookOnlyDTO> Books);
