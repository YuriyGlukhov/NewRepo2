using ASP.Seminar1.Abstraction;
using ASP.Seminar1.Models;
using ASP.Seminar1.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ASP.Seminar1.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _repository;

        public CategoryController(ICategoryRepository repository)
        {
            _repository = repository;
        }
        [HttpGet("get_category")]
        public IActionResult GetCategory()
        {
            var category = _repository.GetGroups();
            return Ok(category);  

        }

        [HttpPost("add_category")]
        public IActionResult AddCategory([FromBody] CategoryModel category)
        {
            var result = _repository.AddGroup(category);

            return Ok(result);
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
