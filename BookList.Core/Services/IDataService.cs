using BookList.Core.DTO;

namespace BookList.Core.Services;

public interface IDataService
{
    Task<AuthorDTO> GetAuthorAsync(int id);
    Task<IEnumerable<AuthorDTO>> GetAuthorsAsync();
    Task<BookDTO> GetBookAsync(int id);
    Task<IEnumerable<BookDTO>> GetBooksAsync();
    Task<GenreDTO> GetGenreAsync(int id);
    Task<IEnumerable<GenreDTO>> GetGenresAsync();
}