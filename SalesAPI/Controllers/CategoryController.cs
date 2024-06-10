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

        [HttpGet("read by category name/{categoryName}")]
        public IActionResult Read(string categoryName)
        {
            var product = _categoryRepository.Read(categoryName);
            if (product == null)
            {
                return NotFound("Category not found");
            }
            return Ok(product);
        }
        [HttpGet("read by Id/{id}")]
        public async Task<IActionResult> ReadCategoryById(int id)
        {
            var category = await _categoryRepository.ReadCategoryById(id);
            if (category == null)
            {
                return NotFound("Category not found.");
            }
            return Ok(category);
        }

        [HttpGet("read all categories")]
        public async Task<IActionResult> ReadAllCategories()
        {
            var categories = await _categoryRepository.ReadAllCategories();
            return Ok(categories);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDTO categoryDTO)
        {
            if (categoryDTO == null || string.IsNullOrWhiteSpace(categoryDTO.CategoryName))
            {
                return BadRequest("Category name cannot be empty.");
            }

            var category = await _categoryRepository.ReadCategoryById(id);
            if (category == null)
            {
                return NotFound("Category not found.");
            }

            category.CategoryName = categoryDTO.CategoryName;

            await _categoryRepository.UpdateCategory(category);

            return Ok("Category updated successfully.");
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryRepository.ReadCategoryById(id);
            if (category == null)
            {
                return NotFound("Category not found.");
            }

            await _categoryRepository.DeleteCategory(id);

            return Ok("Category deleted successfully.");
        }
    }
}

