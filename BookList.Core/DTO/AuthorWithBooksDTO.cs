namespace BookList.Core.DTO;

public record AuthorWithBooksDTO(int Id, string Name, bool IsFavorite, 
    IEnumerable<BookOnlyDTO> Books);
