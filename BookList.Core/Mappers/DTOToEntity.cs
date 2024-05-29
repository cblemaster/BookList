using BookList.Core.DTO;
using BookList.Core.Entities;

namespace BookList.Core.Mappers;

public static class DTOToEntity
{
    public static Author MapCreateUpdateAuthorDTOToAuthorEntity(CreateUpdateAuthorDTO dto, Author entity = null!)
    {
        if (entity is not null)
        {
            entity.Name = dto.Name;
            entity.IsFavorite = dto.IsFavorite;
            return entity;
        }
        
        return new()
        {
            Id = dto.Id,
            Name = dto.Name,
            IsFavorite = dto.IsFavorite,
        };
    }

    public static Genre MapCreateUpdateGenreDTOToGenreEntity(CreateUpdateGenreDTO dto, Genre entity = null!)
    {
        if (entity is not null)
        {
            entity.Name = dto.Name;
            entity.IsFavorite = dto.IsFavorite;
            return entity;
        }

        return new()
        {
            Id = dto.Id,
            Name = dto.Name,
            IsFavorite = dto.IsFavorite,
        };
    }

    public static Book MapCreateUpdateBookDTOToBookEntity(CreateUpdateBookDTO dto, Book entity = null!)
    {
        if (entity is not null)
        {
            entity.Title = dto.Title;
            entity.Subtitle = dto.Subtitle;
            entity.IsFavorite = dto.IsFavorite;
            entity.GenreId = dto.Genre.Id;
            entity.Publisher = dto.Publisher;
            entity.PageCount = dto.PageCount;
            entity.Description = dto.Description;
            return entity;
        }

        return new()
        {
            Id = dto.Id,
            Title = dto.Title,
            Subtitle = dto.Subtitle,
            IsFavorite = dto.IsFavorite,
            GenreId = dto.Genre.Id,
            Publisher = dto.Publisher,
            PageCount = dto.PageCount,
            Description = dto.Description,
        };
    }
}
