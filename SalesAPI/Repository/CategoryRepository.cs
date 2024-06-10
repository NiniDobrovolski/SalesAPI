using Microsoft.EntityFrameworkCore;
using SalesAPI.Data;
using SalesAPI.Interfaces;
using SalesAPI.Models;

namespace SalesAPI.Repository
{
	public class CategoryRepository : ICategoryRepository
	{
        private readonly SalesDbContext _context;
        public CategoryRepository(SalesDbContext context)
        {
            _context = context;
        }

        public async Task CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public ICollection<Product> Read(string categoryName)
        {
            int? categoryId = _context.Categories.FirstOrDefault(i => i.CategoryName == categoryName)?.CategoryID;

            if (categoryId == null)
            {
                return null;
            }

            return _context.Products.Where(i => i.CategoryID == categoryId).ToList();
        }

        public async Task<Category> ReadCategoryById(int categoryId)
        {
            return await _context.Categories.FindAsync(categoryId);
        }

        public async Task<IEnumerable<Category>> ReadAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategory(int categoryId)
        {
            var category = await ReadCategoryById(categoryId);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

    }
}

