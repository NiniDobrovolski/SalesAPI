using SalesAPI.Models;
namespace SalesAPI.Interfaces
{
	public interface ICategoryRepository
	{
        Task CreateCategory(Category category);
        ICollection<Product> Read(string categoryName);
        Task<Category> ReadCategoryById(int categoryId);
        Task<IEnumerable<Category>> ReadAllCategories();
        Task UpdateCategory(Category category);
        Task DeleteCategory(int categoryId);
    }
}

