using SalesAPI.Models;
using SalesAPI.DTO;

namespace SalesAPI.Interfaces
{
    public interface IProductRepository
    {
        Task Create(ProductDTO product);
        Product Read(int id);
        void Update(int id, ProductDTO productDTO);
        void Delete(int id);
        void DecreaseQuantity(int productId, int quantity);
        Task<Product> SearchByName(string name);
        Task<IEnumerable<Product>> SearchByPartialName(string name);
    }
}
