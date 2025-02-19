using ASP.Seminar1.Models.DTO;
using Microsoft.Extensions.Caching.Memory;

namespace ASP.Seminar1.Abstraction
{
    public interface ICategoryRepository
    {
        public int AddGroup(CategoryModel category);
        public IEnumerable<CategoryModel> GetGroups();
        public MemoryCacheStatistics GetCacheStatisticUrl();
    }
}
