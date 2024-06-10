using SalesAPI.Data;
using SalesAPI.Interfaces;
using SalesAPI.Models;
using SalesAPI.DTO;

namespace SalesAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly SalesDbContext _context;

        public ProductRepository(SalesDbContext context)
        {
            _context = context;
        }

        public void Create(ProductDTO product)
        {
            Product productToAdd = new Product();
            productToAdd.CategoryID = product.CategoryID;
            productToAdd.Name = product.Name;
            productToAdd.Price = product.Price;
            productToAdd.Qty = product.Qty;
            _context.Products.Add(productToAdd);
            _context.SaveChanges();
        }

        public Product Read(int id)
        {
            return _context.Products.Find(id);
        }

        public IEnumerable<Product> SearchByName(string name)
        {
            return _context.Products
                             .Where(p => p.Name.Contains(name))
                             .ToList();
        }

        public void Update(int id, ProductDTO productDTO)
        {
            var existingProduct = _context.Products.FirstOrDefault(p => p.ProductID == id);
            if (existingProduct != null)
            {
                existingProduct.Name = productDTO.Name;
                existingProduct.Qty = productDTO.Qty;
                existingProduct.Price = productDTO.Price;
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductID == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        public void DecreaseQuantity(int productId, int quantity)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                if (product.Qty >= quantity)
                {
                    product.Qty -= quantity;
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("Insufficient product quantity");
                }
            }
        }

    }
}
