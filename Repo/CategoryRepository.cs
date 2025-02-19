using ASP.Seminar1.Abstraction;
using ASP.Seminar1.Models;
using ASP.Seminar1.Models.DTO;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics.Eventing.Reader;

namespace ASP.Seminar1.Repo
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly ProductContext _context;

        public CategoryRepository (IMapper mapper, IMemoryCache memoryCache, ProductContext context)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _context = context;
        }
        public int AddGroup(CategoryModel category)
        {
            using (_context)
            {
                var entCategory = _context.Categories.FirstOrDefault(x => x.Name.ToLower() == category.Name.ToLower());
                if (entCategory == null)
                {
                    entCategory = _mapper.Map<Category>(category);
                    _context.Categories.Add(entCategory);
                    _context.SaveChanges();

                    _cache.Remove("categories");
                }
                return entCategory.Id;
            }
        }

        public IEnumerable<CategoryModel> GetGroups()
        {
            if (_cache.TryGetValue("categories", out List<CategoryModel> category))
            {
                return category;
            }

            using (_context)
            {
                var categories = _context.Categories.Select(x => _mapper.Map<CategoryModel>(x)).ToList();
                _cache.Set("categories", categories, TimeSpan.FromMinutes(30));
                return categories;
            }
        }

        public MemoryCacheStatistics GetCacheStatisticUrl()
        {
            var cacheStat = _cache.GetCurrentStatistics();

            return cacheStat;
        }
    }
}
