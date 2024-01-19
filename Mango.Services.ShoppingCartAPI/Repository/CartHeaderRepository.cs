using System;
using System.Linq.Expressions;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.Repository
{
	public class CartHeaderRepository : ICartHeader
	{
        private readonly AppDbContext _db;

        public CartHeaderRepository(AppDbContext db)
		{
            _db = db;
        }

        public async Task CreateHeaderAsync(CartHeader entity)
        {
            await _db.CartHeaders.AddAsync(entity);

            await _db.SaveChangesAsync();
        }

        public async Task DeleteCartHeaderAsync(CartHeader entity)
        {
            _db.CartHeaders.Remove(entity);

            await _db.SaveChangesAsync();
        }

        public async Task<List<CartHeader>> GetAllCartHeaderAsync(Expression<Func<CartHeader, bool>> filter = null)
        {
            IQueryable<CartHeader> query = _db.CartHeaders;

            if (filter != null)
            {
                query = query.AsNoTracking().Where(filter);
            }

            //if (!tracked)
            //{
            //    query = query.AsNoTracking();
            //}

            return await query.ToListAsync();
        }

        public async Task<CartHeader> GetCartHeaderAsync(Expression<Func<CartHeader, bool>> filter = null)
        {
            IQueryable<CartHeader> query = _db.CartHeaders;

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

        public async Task<CartHeader> UpdateCartHeaderAsync(CartHeader entity)
        {
            _db.CartHeaders.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}

