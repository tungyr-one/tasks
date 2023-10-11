using Dapper;
using OnlineStoreApp.Data;
using OnlineStoreApp.Models;

namespace OnlineStoreApp.Repositories
{
    public class ProductRepository:IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProductAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var product = await connection.QueryFirstOrDefaultAsync<Product>($"SELECT * FROM products WHERE id = {id}");
            return product;
        }

        public async Task<Product[]> GetAllProductsAsync()
        {
            using var connection = _context.CreateConnection();
            return (await connection.QueryAsync<Product>("SELECT * FROM Products")).ToArray();
        }

        public async Task<int> AddProductAsync(Product product)
        {
            using var connection = _context.CreateConnection();
            var newProductId = await connection
            .ExecuteScalarAsync<int>($@"INSERT INTO products (Name, Price) 
                                        VALUES ('{product.Name}', {product.Price}) RETURNING Id");
            return newProductId;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            using var connection = _context.CreateConnection();

            var affectedRows = await connection
            .ExecuteAsync($"DELETE FROM Products WHERE Id = {productId}");

            return affectedRows > 0;
        }

        public async Task<bool> UpdateProductAsync(int id, Product product)
        {
            using var connection = _context.CreateConnection();

            var affectedRows = await connection
            .ExecuteAsync($@"UPDATE Products 
                            SET Name = '{product.Name}', Price = {product.Price} 
                            WHERE Id = {id}");

            return affectedRows > 0;
        }
    }
}