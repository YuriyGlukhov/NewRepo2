using ASP.Seminar1.Abstraction;
using ASP.Seminar1.Models;
using ASP.Seminar1.Models.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics.Eventing.Reader;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;


namespace ASP.Seminar1.Repo
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly ProductContext _context;

        public ProductRepository(IMapper mapper, IMemoryCache memoryCache, ProductContext context)
        {
            _mapper = mapper;
            _cache = memoryCache;
            _context = context;
        }
        public int AddProduct(ProductModel product)
        {
            using (_context)
            {
                var entProduct = _context.Products.FirstOrDefault(x => x.Name.ToLower() == product.Name.ToLower());
                if (entProduct == null)
                {
                    entProduct = _mapper.Map<Product>(product);
                    _context.Products.Add(entProduct);
                    _context.SaveChanges();

                    _cache.Remove("products");
                }
                return entProduct.Id;
            }
        }
 

        public IEnumerable<ProductModel> GetProducts()
        {
            if (_cache.TryGetValue("products", out List<ProductModel> product))
            {
                return product;
            }
            using (_context)
            {
                var products = _context.Products.Select(x => _mapper.Map<ProductModel>(x)).ToList();
                _cache.Set("\"products", products, TimeSpan.FromMinutes(30));
                return products;
            }
        }

        public string GetProductCSV()
        {
            using (_context)
            {
                var product = _context.Products.OrderBy(or => or.Id).Select(x => new ProductModel { Id = x.Id, Name = x.Name }).ToList();
                var content = GetCSV(product);

                return content;
            }
        }
        public string GetCSV(List<ProductModel> products)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var product in products)
            {
                sb.AppendLine($"ID: {product.Id} - Product: {product.Name}\n");
            }
            return sb.ToString();
        }

        public MemoryCacheStatistics GetCacheStatisticUrl()
        {
            var cacheStat = _cache.GetCurrentStatistics();

            return cacheStat;
        }
    }
}
