﻿using BookList.Core.DTO;
using BookList.Core.Entities;

namespace BookList.Core.Mappers;

public static class EntityToDTO
{
    public static GenreDTO MapGenreEntityToDTO(Genre entity)
    {
        return new(entity.Id, entity.Name, entity.IsFavorite);
    }

    public static IEnumerable<GenreDTO> MapGenreEntitiesToDTOs(IEnumerable<Genre> entities)
    {
        List<GenreDTO> dtos = [];

        foreach (Genre entity in entities)
        {
            dtos.Add(MapGenreEntityToDTO(entity));
        }
        return dtos.AsEnumerable();
    }

    public static AuthorDTO MapAuthorEntityToDTO(Author entity)
    {
        return new(entity.Id, entity.Name, entity.IsFavorite);
    }

    public static IEnumerable<AuthorDTO> MapAuthorEntitiesToDTOs(IEnumerable<Author> entities)
    {
        List<AuthorDTO> dtos = [];

        foreach (Author entity in entities)
        {
            dtos.Add(MapAuthorEntityToDTO(entity));
        }
        return dtos.AsEnumerable();
    }

    public static BookDTO MapBookEntityToDTO(Book entity)
    {
        return new(entity.Id, entity.Title, entity.Subtitle, entity.IsFavorite,
            entity.Publisher, entity.PageCount, entity.Description, 
            MapGenreEntityToDTO(entity.Genre), MapAuthorEntitiesToDTOs(entity.Authors));
    }

    public static IEnumerable<BookDTO> MapBookEntitiesToDTOs(IEnumerable<Book> entities)
    {
        List<BookDTO> dtos = [];

        foreach (Book entity in entities)
        {
            dtos.Add(MapBookEntityToDTO(entity));
        }
        return dtos.AsEnumerable();
    }
}