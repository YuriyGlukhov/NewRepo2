using ASP.Seminar1.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ASP.Seminar1.Abstraction
{
    public interface IProductRepository
    { 
        public int AddProduct(ProductModel product);
        public IEnumerable<ProductModel> GetProducts();
        public MemoryCacheStatistics GetCacheStatisticUrl();
        public string GetProductCSV();
    }
}
