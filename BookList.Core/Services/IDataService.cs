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
    Task DeleteBookAsync(int id);
    Task DeleteGenreAsync(int id);
    Task DeleteAuthorAsync(int id);
    Task<bool> DoesGenreHaveBooks(int id);
    Task<bool> DoesAuthorHaveBooks(int id);
    Task<AuthorDTO> CreateAuthorAsync(CreateUpdateAuthorDTO authorToCreate);
}