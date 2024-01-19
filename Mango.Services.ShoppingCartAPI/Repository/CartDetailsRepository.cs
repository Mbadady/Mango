using System;
using System.Linq.Expressions;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.Repository
{
	public class CartDetailsRepository : ICartDetails
	{
        private readonly AppDbContext _db;

        public CartDetailsRepository(AppDbContext db)
		{
            _db = db;
        }

        public async Task CreateCartDetailsAsync(CartDetails entity)
        {
            await _db.CartDetails.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<CartDetails> GetCartDetailsAsync(Expression<Func<CartDetails, bool>> filter = null)
        {
            IQueryable<CartDetails> query = _db.CartDetails;

            if(filter != null)
            {
                query = query.AsNoTracking().Where(filter);
            }

            //if (!tracked)
            //{
            //    query = query.AsNoTracking();
            //}

            return await query.FirstOrDefaultAsync();
        }

        public async Task DeleteCartDetailsAsync(CartDetails entity)
        {
            _db.CartDetails.Remove(entity);

            await _db.SaveChangesAsync();
        }

       

        public async Task<CartDetails> UpdateCartDetailsAsync(CartDetails entity)
        {
             _db.CartDetails.Update(entity);

            await _db.SaveChangesAsync();

            return entity;
        }

        public async Task<int> GetCartDetailsCountAsync(Expression<Func<CartDetails, bool>> filter = null)
        {
            IQueryable<CartDetails> query = _db.CartDetails;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }
    }
}

