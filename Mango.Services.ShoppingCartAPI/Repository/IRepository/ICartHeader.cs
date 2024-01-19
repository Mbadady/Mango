using System;
using System.Linq.Expressions;
using Mango.Services.ShoppingCartAPI.Models;

namespace Mango.Services.ShoppingCartAPI.Repository.IRepository
{
	public interface ICartHeader
	{
		Task<CartHeader> GetCartHeaderAsync(Expression<Func<CartHeader, bool>> filter = null);

        //Task<List<CartHeader>> GetAllCartHeaderAsync(Expression<Func<CartHeader, bool>> filter = null);

        Task CreateHeaderAsync(CartHeader entity);

		Task DeleteCartHeaderAsync(CartHeader entity);

		Task<CartHeader> UpdateCartHeaderAsync(CartHeader entity);

    }
}

