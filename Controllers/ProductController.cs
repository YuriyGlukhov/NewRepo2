using ASP.Seminar1.Abstraction;
using ASP.Seminar1.Models;
using ASP.Seminar1.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace ASP.Seminar1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductController(IProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("get_product")]
        public IActionResult GetProducts()
        {
            var products = _repository.GetProducts();

            return Ok(products);

        }

        [HttpPost("add_product")]
        public IActionResult AddProduct([FromBody] ProductModel product)
        {
            var result = _repository.AddProduct(product);
            return Ok(result);
        }

        [HttpGet("get_product_csv")]

        public FileContentResult GetProductCSV()
        {
            var content = _repository.GetProductCSV();

            return File(new System.Text.UTF8Encoding().GetBytes(content), "text/csv", "report.csv");
           
        }

        [HttpGet("get_cache_statistic_url")]
        public ActionResult<string> GetCacheStatisticUrl()
        {
            var cacheStat = _repository.GetCacheStatisticUrl();


            if (cacheStat != null)
            {
                var statsJson = JsonSerializer.Serialize(cacheStat);


                var directoryPath = Path.Combine(Directory.GetCurrentDirectory());

                var fileName = "cache_" + DateTime.Now.ToBinary().ToString() + ".json";
                var filePath = Path.Combine(directoryPath, fileName);

                System.IO.File.WriteAllText(filePath, statsJson);

                return "https://" + Request.Host.ToString() + "/static/" + fileName;
            }

            return StatusCode(404);
        }


    }
}