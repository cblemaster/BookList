using BookList.Core.DTO;

namespace BookList.Core.Services;

public interface IDataService
{
    Task<AuthorDTO> CreateAuthorAsync(CreateUpdateAuthorDTO authorToCreate);
    Task DeleteAuthorAsync(int id);
    Task<AuthorDTO> GetAuthorAsync(int id);
    Task<AuthorWithBooksDTO> GetAuthorWithBooksAsync(int id);
    Task<IEnumerable<AuthorDTO>> GetAuthorsAsync();
    Task UpdateAuthorAsync(int id, CreateUpdateAuthorDTO dto);
    Task<BookDTO> CreateBookAsync(CreateUpdateBookDTO bookToCreate);
    Task DeleteBookAsync(int id);
    Task<BookDTO> GetBookAsync(int id);
    Task<IEnumerable<BookDTO>> GetBooksAsync();
    Task UpdateBookAsync(int id, CreateUpdateBookDTO dto);
    Task<GenreDTO> CreateGenreAsync(CreateUpdateGenreDTO genreToCreate);
    Task DeleteGenreAsync(int id);
    Task<GenreDTO> GetGenreAsync(int id);
    Task<GenreWithBooksDTO> GetGenreWithBooksAsync(int id);
    Task<IEnumerable<GenreDTO>> GetGenresAsync();
    Task UpdateGenreAsync(int id, CreateUpdateGenreDTO dto);
    Task<bool> IsAuthorNameAlreadyUsed(string name);
    Task<bool> IsGenreNameAlreadyUsed(string name);
}