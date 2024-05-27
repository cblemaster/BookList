using BookList.Core.DTO;
using BookList.Core.Entities;

namespace BookList.Core.Mappers;

public static class EntityToDTO
{
    public static GenreDTO MapGenreEntityToDTO(Genre entity) =>
        new(entity.Id, entity.Name, entity.IsFavorite);

    public static GenreWithBooksDTO MapGenreWithBooksEntityToDTO(Genre entity) =>
        new(entity.Id, entity.Name, entity.IsFavorite, MapBookOnlyEntitiesToDTOs(entity.Books));

    public static IEnumerable<GenreDTO> MapGenreEntitiesToDTOs(IEnumerable<Genre> entities)
    {
        List<GenreDTO> dtos = [];

        foreach (Genre entity in entities)
        {
            dtos.Add(MapGenreEntityToDTO(entity));
        }
        return dtos.AsEnumerable();
    }

    public static AuthorDTO MapAuthorEntityToDTO(Author entity) =>
        new(entity.Id, entity.Name, entity.IsFavorite);

    public static AuthorWithBooksDTO MapAuthorWithBooksEntityToDTO(Author entity) =>
        new(entity.Id, entity.Name, entity.IsFavorite, MapBookOnlyEntitiesToDTOs(entity.Books));

    public static IEnumerable<AuthorDTO> MapAuthorEntitiesToDTOs(IEnumerable<Author> entities)
    {
        List<AuthorDTO> dtos = [];

        foreach (Author entity in entities)
        {
            dtos.Add(MapAuthorEntityToDTO(entity));
        }
        return dtos.AsEnumerable();
    }

    public static BookDTO MapBookEntityToDTO(Book entity) =>
        new(entity.Id, entity.Title, entity.Subtitle, entity.IsFavorite,
            entity.Publisher, entity.PageCount, entity.Description,
            MapGenreEntityToDTO(entity.Genre),
            MapAuthorEntitiesToDTOs(entity.Authors));

    public static BookOnlyDTO MapBookOnlyEntityToDTO(Book entity)
        => new(entity.Id, entity.Title);

    public static IEnumerable<BookDTO> MapBookEntitiesToDTOs(IEnumerable<Book> entities)
    {
        List<BookDTO> dtos = [];

        foreach (Book entity in entities)
        {
            dtos.Add(MapBookEntityToDTO(entity));
        }
        return dtos.AsEnumerable();
    }

    public static IEnumerable<BookOnlyDTO> MapBookOnlyEntitiesToDTOs(IEnumerable<Book> entities)
    {
        List<BookOnlyDTO> dtos = [];

        foreach (Book entity in entities)
        {
            dtos.Add(MapBookOnlyEntityToDTO(entity));
        }
        return dtos.AsEnumerable();
    }
}
