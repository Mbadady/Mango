using System;
using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
	public interface IProductService
    { 

            Task<ResponseDTO?> GetProductByIdAsync(int id);

            Task<ResponseDTO?> GetAllProductAsync();

            Task<ResponseDTO?> CreateProductAsync(ProductDTO productDTO);

            Task<ResponseDTO?> UpdateProductAsync(ProductDTO productDTO);

            Task<ResponseDTO?> DeleteProductAsync(int id);
        
	}
}

