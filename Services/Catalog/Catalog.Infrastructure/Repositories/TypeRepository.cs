
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class TypeRepository : ITypeRepository
    {
        private readonly IMongoCollection<ProductType> _types;
        public TypeRepository(IConfiguration config)
        {
            var client = new MongoClient(config["DataBaseSettings:connectionstring"]);
            var db = client.GetDatabase(config["DataBaseSettings:DatabaseName"]);
            _types = db.GetCollection<ProductType>(config["DataBaseSettings:TypeCollectionName"]);
        }
        public async Task<IEnumerable<ProductType>> GetAllTypes()
        {
            return await _types.Find(_ => true).ToListAsync();
        }

        public async Task<ProductType> GetTypeAsync(string Id)
        {
            return await _types.Find(x=>x.Id == Id).FirstOrDefaultAsync();
        }
    }
}
