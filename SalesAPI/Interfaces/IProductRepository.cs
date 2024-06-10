using SalesAPI.Models;
using SalesAPI.DTO;

namespace SalesAPI.Interfaces
{
    public interface IProductRepository
    {
        void Create(ProductDTO product);
        Product Read(int id);
        void Update(int id, ProductDTO productDTO);
        void Delete(int id);
        void DecreaseQuantity(int productId, int quantity);
        IEnumerable<Product> SearchByName(string name);
    }
}
