using System;
using System.Linq.Expressions;
using Mango.Services.ProductAPI.Model;
using Mango.Services.ProductAPI.Model.DTO;

namespace Mango.Services.ProductAPI.Repository.IRepository
{
	public interface IProductRepository
	{
		Task CreateProductAsync(Product entity);

		Task<Product?> GetProductAsync(Expression<Func<Product, bool>> filter = null);

		Task<List<Product>> GetAllProductAsync(Expression<Func<Product, bool>> filter = null);

		Task<Product?> UpdateProductAsync(int productId,Product entity);

		Task<Product?> RemoveProductAsync(Product entity);
	}
}

