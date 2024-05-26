using BookList.Core.DTO;
using BookList.Core.Entities;

namespace BookList.Core.Mappers;

public static class DTOToEntity
{
    public static Author MapCreateUpdateAuthorDTOToAuthorEntity(CreateUpdateAuthorDTO dto)
    {
        return new()
        {
            Id = dto.Id,
            Name = dto.Name,
            IsFavorite = dto.IsFavorite,
        };
    }
}
