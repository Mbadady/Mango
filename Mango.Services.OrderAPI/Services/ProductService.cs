using System;
using System.Text.Json;
using Mango.Services.OrderAPI.Models.DTO;
using Mango.Services.OrderAPI.Services.IServices;

namespace Mango.Services.OrderAPI.Services
{
	public class ProductService : IProductService
	{
        private readonly IHttpClientFactory _httpClientFactory;
        //private string _productUrl;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            //_productUrl = configuration.GetValue<string>("ServiceUrls:ProductAPI");
        }

        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("Product");

            var response = await client.GetAsync($"/api/product");

            var resp = await response.Content.ReadFromJsonAsync<ResponseDTO>();

            if (resp != null && resp.IsSuccess)
            {
                return JsonSerializer.Deserialize<IEnumerable<ProductDTO>>(Convert.ToString(resp.Result));
            }

            return new List<ProductDTO>();
        }
    }
}

