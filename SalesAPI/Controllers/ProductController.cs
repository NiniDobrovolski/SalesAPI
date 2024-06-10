using Microsoft.AspNetCore.Mvc;
using SalesAPI.Interfaces;
using SalesAPI.DTO;

namespace SalesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(ProductDTO product)
        {
            if (_productRepository.SearchByName(product.Name) != null)
            {
                return BadRequest("Product name already taken");
            }
            _productRepository.Create(product);
            return Ok("Success");
        }

        [HttpGet("search product by id/{id}")]
        public IActionResult Read(int id)
        {
            var product = _productRepository.Read(id);
            if (product == null)
            {
                return NotFound("Product not found");
            }
            return Ok(product);
        }

        [HttpGet("search product by name/{name}")]
        public IActionResult SearchByName(string name)
        {
            var products = _productRepository.SearchByName(name);
            if (products == null || !products.Any())
            {
                return NotFound("No products found with the given name");
            }
            return Ok(products);
        }



        [HttpPut("update/{id}")]
        public IActionResult Update(int id, [FromBody] ProductDTO productDTO)
        {
            var product = _productRepository.Read(id);  
            if (id != product.ProductID)
            {
                return BadRequest("Product ID mismatch");
            }

            var existingProduct = _productRepository.Read(id);
            if (existingProduct == null)
            {
                return NotFound("Product not found");
            }

            _productRepository.Update(id, productDTO);
            return Ok("Success");
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var product = _productRepository.Read(id);
            if (product == null)
            {
                return NotFound("Product not found");
            }
            _productRepository.Delete(id);
            return Ok("Success");
        }
    }
}
