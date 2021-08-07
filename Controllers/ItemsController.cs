using CatalogService.DB.Entities;
using CatalogService.DB.Repository;
using CatalogService.DTO.Dtos;
using CatalogService.Extension;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.Controllers
{
    /// <summary>
    /// https://{ip}:{port}/items
    /// </summary>
    [ApiController]
    [Route("Items")]
    public class ItemsController : ControllerBase
    {
        private readonly IitemsRepository _itemsRepository;

        public ItemsController(IitemsRepository itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemDtos()
        {
            var items = (await _itemsRepository.GetAll()).Select(a => a.AsDto());
            return items;

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemDtosById(Guid id)
        {
            var res = await _itemsRepository.GetById(id);
            if (res == null)
            {
                return NotFound();
            }
            return res.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostItemDto(CreateItemDto i)
        {
            Item item = new Item { Name = i.Name, Description = i.Description, Price = i.Price, CreatedDate = DateTimeOffset.UtcNow };
            await _itemsRepository.Create(item);
            return CreatedAtAction(nameof(GetItemDtosById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ItemDto>> PutItemDto(Guid id, UpdateItemDto i)
        {
            Item item = await _itemsRepository.GetById(id);

            if (item == null)
            {
                return NotFound();
            }

            item.Name = i.Name;
            item.Description = i.Description;
            item.Price = i.Price;

            await _itemsRepository.Update(item);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ItemDto>> DeleteItemDto(Guid id)
        {
            Item item = await _itemsRepository.GetById(id);

            if (item == null)
            {
                return NotFound();
            }

            await _itemsRepository.Remove(item);

            return NoContent();
        }
    }
}
