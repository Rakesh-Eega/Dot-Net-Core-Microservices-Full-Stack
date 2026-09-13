

using Catalog.Core.Entities;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data
{
    public class DatabaseSeeder
    {
        public static async Task SeedAsync(IOptions<DatabaseSettings> options)
        {
            var settings = options.Value;
            var client = new MongoClient(settings.connectionstring);
            var db = client.GetDatabase(settings.DatabaseName);
            var _products = db.GetCollection<Product>(settings.ProductCollectionName);
            var _brands = db.GetCollection<ProductBrand>(settings.BrandCollectionName);
            var _types = db.GetCollection<ProductType>(settings.TypeCollectionName);

            var seedbasepath = Path.Combine("Data", "SeedData");
            //seed Brands
            List<ProductBrand> brandslist = new List<ProductBrand>();
            if(await _brands.CountDocumentsAsync(_=>true) == 0)
            {
                var branddata = await File.ReadAllTextAsync(Path.Combine(seedbasepath, "brands.json"));
                brandslist = JsonSerializer.Deserialize<List<ProductBrand>>(branddata);
                await _brands.InsertManyAsync(brandslist);
            }
            else
            {
                brandslist = await _brands.Find(_ => true).ToListAsync();
            }
            //seed types
            List<ProductType> Typelist = new List<ProductType>();
            if (await _types.CountDocumentsAsync(_ => true) == 0)
            {
                var typedata = await File.ReadAllTextAsync(Path.Combine(seedbasepath, "types.json"));
                Typelist = JsonSerializer.Deserialize<List<ProductType>>(typedata);
                await _types.InsertManyAsync(Typelist);
            }
            else
            {
                Typelist =await _types.Find(_=>true).ToListAsync();
            }

            //seed products
            List<Product> Productlist = new List<Product>();
            if (await _products.CountDocumentsAsync(_ => true) == 0)
            {
                var productdata = await File.ReadAllTextAsync(Path.Combine(seedbasepath, "products.json"));
                Productlist = JsonSerializer.Deserialize<List<Product>>(productdata);
                foreach(var product in Productlist)
                {
                    //reset Id to let Mongo generate one
                    product.Id = null;
                    if(product.CreatedDate == default)
                    {
                        product.CreatedDate = DateTime.UtcNow;
                    }
                }
                
                await _products.InsertManyAsync(Productlist);
            }
            else
            {
                Productlist = await _products.Find(_ => true).ToListAsync();
               
            }
        }
    }
}
