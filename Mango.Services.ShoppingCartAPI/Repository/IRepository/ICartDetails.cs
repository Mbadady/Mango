using System;
using Mango.Services.ShoppingCartAPI.Models;
using System.Linq.Expressions;

namespace Mango.Services.ShoppingCartAPI.Repository.IRepository
{
	public interface ICartDetails
	{
        Task<CartDetails> GetCartDetailsAsync(Expression<Func<CartDetails, bool>> filter = null);

        Task<int> GetCartDetailsCountAsync(Expression<Func<CartDetails, bool>> filter = null);

        Task CreateCartDetailsAsync(CartDetails entity);

        Task<CartDetails> UpdateCartDetailsAsync(CartDetails entity);

        Task DeleteCartDetailsAsync(CartDetails entity);

    }
}

