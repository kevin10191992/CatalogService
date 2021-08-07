using System;
using System.ComponentModel.DataAnnotations;

namespace CatalogService.DTO.Dtos
{
    public record ItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate);
    public record CreateItemDto([Required] string Name, [Required] string Description, [Required][Range(0, (double)Decimal.MaxValue)] decimal Price);
    public record UpdateItemDto([Required] string Name, string Description, [Range(0, (double)Decimal.MaxValue)] decimal Price);
}