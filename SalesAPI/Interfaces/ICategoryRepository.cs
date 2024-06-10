using SalesAPI.Models;
namespace SalesAPI.Interfaces
{
	public interface ICategoryRepository
	{
        Task CreateCategory(Category category);
        ICollection<Product> Read(string categoryName);
    }
}

