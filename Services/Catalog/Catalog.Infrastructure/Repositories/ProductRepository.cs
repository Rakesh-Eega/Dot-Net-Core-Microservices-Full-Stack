

using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specifications;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Xml.Linq;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _products;
        private readonly IMongoCollection<ProductBrand> _brands;
        private readonly IMongoCollection<ProductType> _types;

        public ProductRepository(IOptions<DatabaseSettings> options)
        {
            var settings = options.Value;
            var client = new MongoClient(settings.connectionstring);
            var db = client.GetDatabase(settings.DatabaseName);
            _products = db.GetCollection<Product>(settings.ProductCollectionName);
            _brands = db.GetCollection<ProductBrand>(settings.BrandCollectionName);
            _types = db.GetCollection<ProductType>(settings.TypeCollectionName);

        }
        public async Task<Product> CreateProduct(Product product)
        {
            await _products.InsertOneAsync(product);
            return product;
        }

        public async Task<bool> DeleteProduct(string productId)
        {
            var deleteproduct = await _products.DeleteOneAsync(p=>p.Id == productId);
            return deleteproduct.IsAcknowledged && deleteproduct.DeletedCount > 0;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _products.Find(_=>true).ToListAsync();
        }

        public async Task<ProductBrand> GetBrandByIdAsync(string brandId)
        {
            return await _brands.Find(x=>x.Id == brandId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            var filter = Builders<Product>.Filter.Regex(p => p.Name, new BsonRegularExpression($".*{name}.*", "i"));
            return await _products.Find(filter).ToListAsync();

        }

        public async Task<IEnumerable<Product>> GetProductByBrand(string name)
        {
            return await _products.Find(x => x.Brand.Name.ToLower() == name.ToLower()).ToListAsync();

        }

        public async Task<Product> GetProduct(string productId)
        {
            return await _products.Find(p => p.Id == productId).FirstOrDefaultAsync();
            ;
        }

        public async Task<Pagination<Product>> GetProducts(CatalogSpecParams specParams)
        {
            var builder = Builders<Product>.Filter;
            var filter = builder.Empty;
            if (!string.IsNullOrEmpty(specParams.Search))
            {
                filter &= builder.Where(p=>p.Name.ToLower().Contains(specParams.Search.ToLower()));
            }
            if (!string.IsNullOrEmpty(specParams.BrandId))
            {
                filter &= builder.Eq(p => p.Brand.Id, specParams.BrandId);
            }
            if (!string.IsNullOrEmpty(specParams.TypeID))
            {
                filter &= builder.Eq(p => p.Type.Id, specParams.TypeID);
            }

            var totalitems = await _products.CountDocumentsAsync(filter);
            var data = await ApplyDataFilters(specParams, filter);
            return new Pagination<Product>(
                specParams.PageIndex,
                specParams.PageSize,
                (int)totalitems,
                data
                );
        }

        public async Task<ProductType> GetTypeByIdAsync(string TypeId)
        {
            return await _types.Find(x => x.Id == TypeId).FirstOrDefaultAsync();

        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateproduct = await _products.ReplaceOneAsync(p=>p.Id ==product.Id,product);
            return updateproduct.IsAcknowledged && updateproduct.ModifiedCount > 0;
        }

        private async Task<IReadOnlyCollection<Product>> ApplyDataFilters(CatalogSpecParams specParams, FilterDefinition<Product> filter)
        {
            var sortdef = Builders<Product>.Sort.Ascending("Name");
            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                sortdef = specParams.Sort switch
                {
                    "priceAsc"=> Builders<Product>.Sort.Ascending(p=>p.Price),
                    "priceDesc"=> Builders<Product>.Sort.Descending(p => p.Price),
                    _=> Builders<Product>.Sort.Ascending(p=>p.Price)
                };
            }
            return await _products.Find(filter).Sort(sortdef)
                .Skip(specParams.PageSize * (specParams.PageIndex - 1))
                .Limit(specParams.PageSize)
                .ToListAsync();
        }
    }
}
