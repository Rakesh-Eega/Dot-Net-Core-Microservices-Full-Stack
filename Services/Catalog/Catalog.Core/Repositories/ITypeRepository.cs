

using Catalog.Core.Entities;

namespace Catalog.Core.Repositories
{
    public interface ITypeRepository
    {
        Task<IEnumerable<ProductType>> GetAllTypes();
        Task<ProductType> GetTypeAsync(string Id);
    }
}
