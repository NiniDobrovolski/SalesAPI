using Microsoft.AspNetCore.Mvc;
using SalesAPI.Data;
using SalesAPI.DTO;
using SalesAPI.Interfaces;
using SalesAPI.Models;

namespace SalesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly SalesDbContext _salesDbContext;

        public CategoryController(ICategoryRepository categoryRepository,SalesDbContext salesDbContext)
        {
            _categoryRepository = categoryRepository;
            _salesDbContext = salesDbContext;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO categoryDTO)
        {
            if (categoryDTO == null || string.IsNullOrWhiteSpace(categoryDTO.CategoryName))
            {
                return BadRequest("Category name cannot be empty.");
            }

            var existingCategory = _salesDbContext.Categories.FirstOrDefault(c => c.CategoryName == categoryDTO.CategoryName);
            if (existingCategory!=null)
            {
                return BadRequest($"{categoryDTO.CategoryName} already exists");
            }


            var category = new Category
            {
                CategoryName = categoryDTO.CategoryName
            };

            await _categoryRepository.CreateCategory(category);

            return Ok("Category created successfully.");
        }

        [HttpGet("{categoryName}")]
        public IActionResult Read(string categoryName)
        {
            var product = _categoryRepository.Read(categoryName);
            if (product == null)
            {
                return NotFound("Category not found");
            }
            return Ok(product);
        }
    }
}

