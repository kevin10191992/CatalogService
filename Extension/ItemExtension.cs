using CatalogService.DB.Entities;
using CatalogService.DTO.Dtos;

namespace CatalogService.Extension
{
    public static class ItemExtension
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
        }
    }
}
