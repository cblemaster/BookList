using BookList.Core.DTO;

namespace BookList.Core.Services;

public interface IDataService
{
    Task<AuthorDTO> CreateAuthorAsync(CreateUpdateAuthorDTO authorToCreate);
    Task DeleteAuthorAsync(int id);
    Task<AuthorDTO> GetAuthorAsync(int id);
    Task<IEnumerable<AuthorDTO>> GetAuthorsAsync();
    Task UpdateAuthorAsync(int id, CreateUpdateAuthorDTO dto);
    Task DeleteBookAsync(int id);
    Task<BookDTO> GetBookAsync(int id);
    Task<IEnumerable<BookDTO>> GetBooksAsync();
    Task<GenreDTO> CreateGenreAsync(CreateUpdateGenreDTO genreToCreate);
    Task DeleteGenreAsync(int id);
    Task<GenreDTO> GetGenreAsync(int id);
    Task<IEnumerable<GenreDTO>> GetGenresAsync();
    Task UpdateGenreAsync(int id, CreateUpdateGenreDTO dto);
    Task<bool> DoesAuthorHaveBooks(int id);
    Task<bool> DoesGenreHaveBooks(int id);
    Task<bool> IsAuthorNameAlreadyUsed(string name);
    Task<bool> IsGenreNameAlreadyUsed(string name);
}