using OnlineStoreApp.Models;

namespace OnlineStoreApp.Repositories
{
    public interface IProductRepository
    {
        ///<summary>
        /// Gets product
        ///</summary>
         public Task<Product> GetProductAsync(int id);
         
        ///<summary>
        /// Gets products array
        ///</summary>
         public Task<Product[]> GetAllProductsAsync();
         
        ///<summary>
        /// Creates product
        ///</summary>
         public Task<int> AddProductAsync(Product product);
         
        ///<summary>
        /// Updates product
        ///</summary>
         public Task<bool> UpdateProductAsync(int id, Product product);
         
        ///<summary>
        /// Deletes product
        ///</summary>
         public Task<bool> DeleteProductAsync(int productId);
    }
}