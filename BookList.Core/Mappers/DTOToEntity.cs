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

    public static Genre MapCreateUpdateGenreDTOToAuthorEntity(CreateUpdateGenreDTO dto, Genre entity = null!)
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
}
