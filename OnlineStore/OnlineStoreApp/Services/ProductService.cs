using System.Text.Json.Nodes;
using AutoMapper;
using OnlineStoreApp.Models;
using OnlineStoreApp.Repositories;


namespace OnlineStoreApp.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _repository;

        public ProductService(IMapper mapper, IProductRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ProductDto> GetProductAsync(int id)
        {
            var product = await _repository.GetProductAsync(id);
            var productDto = _mapper.Map<Product, ProductDto>(product);
            var btcRate = await GetBitcoinRate();
            
            productDto.PriceBtc = product.Price / btcRate;

            return productDto;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _repository.GetAllProductsAsync();
            var prodsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);

            var btcRate = await GetBitcoinRate();
            
            foreach(var product in prodsDto)
            {
                product.PriceBtc = product.Price / btcRate;
            }

            return prodsDto;
        }

        public async Task<IResult> AddProductAsync(Product product)
        {       
            var newProductId = await _repository.AddProductAsync(product);
            if (newProductId > 0)
            {   
                var productDto = _mapper.Map<ProductDto>(product);
                productDto.Id = newProductId;   

                var btcRate = await GetBitcoinRate();
                productDto.PriceBtc = product.Price / btcRate;
                return Results.Ok(productDto);
            }

            return Results.BadRequest("Can't add product");       
        }

        public async Task<IResult> UpdateProductAsync(int id, Product product)
        {
            if (!await _repository.UpdateProductAsync(id, product)) 
            {
                return Results.BadRequest("Failed to update product");
            }

            var btcRate = await GetBitcoinRate();
            var productDto = _mapper.Map<ProductDto>(product);            
            productDto.PriceBtc = product.Price / btcRate;
            return Results.Ok(productDto);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
           return await _repository.DeleteProductAsync(id);
        }
       
        ///<summary>
        /// Gets bitcoin rate from web resource
        ///</summary>
        private async Task<decimal> GetBitcoinRate()
        {
            HttpClient client = new();

            decimal bitcoinRate = default;
            var path = $"https://api.coindesk.com/v1/bpi/currentprice.json";
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                JsonNode jsonDocument = JsonNode.Parse(jsonResponse)!;
                bitcoinRate = jsonDocument["bpi"]!["USD"]!["rate_float"]!.GetValue<decimal>();     
            }
            return bitcoinRate;
        }
    }
}