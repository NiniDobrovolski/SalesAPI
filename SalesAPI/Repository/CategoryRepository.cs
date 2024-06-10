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

    }
}

