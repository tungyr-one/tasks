using OnlineStoreApp.Models;

namespace OnlineStoreApp.Services
{
    public interface IProductService
    {
        ///<summary>
        /// Gets product
        ///</summary>
        Task<ProductDto> GetProductAsync(int id);

        ///<summary>
        /// Gets products array
        ///</summary>
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();

        ///<summary>
        /// Creates product
        ///</summary>
        Task<IResult> AddProductAsync(Product product);

        ///<summary>
        /// Updates product
        ///</summary>
        Task<IResult> UpdateProductAsync(int id, Product product);

        ///<summary>
        /// Deletes product
        ///</summary>
        Task<bool> DeleteProductAsync(int id);
    }
}