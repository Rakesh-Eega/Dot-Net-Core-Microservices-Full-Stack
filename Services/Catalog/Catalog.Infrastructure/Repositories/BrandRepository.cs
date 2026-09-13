

using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly IMongoCollection<ProductBrand> _brands;
        public BrandRepository(IConfiguration config)
        {
            var client = new MongoClient(config["DataBaseSettings:connectionstring"]);
            var db = client.GetDatabase(config["DataBaseSettings:DatabaseName"]);
            _brands = db.GetCollection<ProductBrand>(config["DataBaseSettings:BrandCollectionName"]);
        }
        public async Task<IEnumerable<ProductBrand>> GetAllBrands()
        {
            return await _brands.Find(_ => true).ToListAsync();
        }

        public async Task<ProductBrand> GetBrandAsync(string id)
        {
           return await _brands.Find(x=>x.Id == id).FirstOrDefaultAsync();
        }
    }
}
