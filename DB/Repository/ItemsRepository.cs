using CatalogService.DB.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.DB.Repository
{
    public interface IitemsRepository
    {
        Task<IReadOnlyCollection<Item>> GetAll();
        Task<Item> GetById(Guid id);
        Task Create(Item item);
        Task Update(Item item);
        Task Remove(Item item);
    }


    public class ItemsRepository: IitemsRepository
    {
        private readonly IConfiguration _configuration;

        private const string CollectionName = "Items";

        private readonly IMongoCollection<Item> DbCollection;

        private readonly FilterDefinitionBuilder<Item> FilterBuilder = Builders<Item>.Filter;

        public ItemsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            var MongoClient = new MongoClient(_configuration.GetConnectionString("DB"));
            var Database = MongoClient.GetDatabase("Catalog");
            DbCollection = Database.GetCollection<Item>(CollectionName);
        }

        public async Task<IReadOnlyCollection<Item>> GetAll()
        {
            var filtro = await DbCollection.FindAsync(FilterBuilder.Empty);
            return await filtro.ToListAsync();
        }

        public async Task<Item> GetById(Guid id)
        {
            var filtro = await DbCollection.FindAsync(FilterBuilder.Eq(a => a.Id, id));
            return await filtro.FirstOrDefaultAsync();
        }


        public async Task Create(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            await DbCollection.InsertOneAsync(item);
        }


        public async Task Update(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var filtro = FilterBuilder.Eq(a => a.Id, item.Id);

            await DbCollection.ReplaceOneAsync(filtro, item);
        }

        public async Task Remove(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var filtro = FilterBuilder.Eq(a => a.Id, item.Id);

            await DbCollection.DeleteOneAsync(filtro);
        }
    }
}
