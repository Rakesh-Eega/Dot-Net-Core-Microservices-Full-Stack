
namespace Catalog.Core.Specifications
{
    public class CatalogSpecParams
    {
        private const int MaxPageSize = 70;
        private int _PageSize = 10;

        public int PageIndex { get; set; } = 1;

        public int PageSize
        {
            get => _PageSize;
            set => _PageSize = (value > MaxPageSize) ? MaxPageSize : value; 
        }

       public string? BrandId { get; set; }
       public string? TypeID { get; set; }
       public string? Sort { get; set; }
       public string? Search { get; set;  }


    }
}
