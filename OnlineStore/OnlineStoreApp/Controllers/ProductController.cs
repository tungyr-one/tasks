using Microsoft.AspNetCore.Mvc;
using OnlineStoreApp.Models;
using OnlineStoreApp.Repositories;
using OnlineStoreApp.Services;

namespace OnlineStoreApp.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController: ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService, IProductRepository repository)
        {
            _productService = productService;
        }

        ///<summary>
        /// Gets product from service, if null returns NotFound
        ///</summary>
        [HttpGet("{id}")]
        public async Task<IResult> GetProductAsync(int id)
        {
            return await _productService.GetProductAsync(id) switch
            {
                null => Results.NotFound(),
                var prod => Results.Ok(prod)
            };
        }

        ///<summary>
        /// Gets products list
        ///</summary>
        [HttpGet]
        public async Task<IResult> GetProductsAsync()
        {
            var products = await _productService.GetAllProductsAsync();
            return Results.Ok(products);
        }

        ///<summary>
        /// Creates product 
        ///</summary>
        [HttpPost]
        public async Task<IResult> AddProductAsync(Product product)
        {
            return await _productService.AddProductAsync(product);      
        }

        ///<summary>
        /// Updates product 
        ///</summary>
        [HttpPut("{id}")]
        public async Task<IResult> UpdateProductAsync(int id, Product product)
        {
            return await _productService.UpdateProductAsync(id, product);
        }

        ///<summary>
        /// Deletes product
        ///</summary>
        [HttpDelete("{id}")]
        public async Task<IResult> DeleteProductAsync(int id)
        {
            if (await _productService.DeleteProductAsync(id)) return Results.Ok();
            return Results.BadRequest("Failed to delete product");            
        }
    }
}