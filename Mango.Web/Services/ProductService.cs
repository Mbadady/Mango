using System;
using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Mango.Web.Utility;

namespace Mango.Web.Services
{
	public class ProductService : IProductService
	{
        private readonly IBaseService _baseService;

        public ProductService(IBaseService baseService)
		{
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> CreateProductAsync(ProductDTO productDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                Url = SD.ProductAPIBase + "/api/product",
                ApiType = SD.ApiType.POST,
                Data = productDTO,
                //AccessToken = ""
            });
        }

        public async Task<ResponseDTO?> DeleteProductAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                Url = SD.ProductAPIBase + "/api/product/" + id,
                ApiType = SD.ApiType.DELETE,
                
                //AccessToken = ""
            });
        }

        public async Task<ResponseDTO?> GetAllProductAsync()
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                Url = SD.ProductAPIBase + "/api/product",
                ApiType = SD.ApiType.GET
                //AccessToken = ""
            });
        }

        public async Task<ResponseDTO?> GetProductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                Url = SD.ProductAPIBase + "/api/product/" + id,
                ApiType = SD.ApiType.GET
                //AccessToken = ""
            });
        }

        public async Task<ResponseDTO?> UpdateProductAsync(ProductDTO productDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                Url = SD.ProductAPIBase + "/api/product/" + productDTO.ProductId,
                ApiType = SD.ApiType.PUT,
                Data = productDTO
                //AccessToken = ""
            });
        }
    }
}

