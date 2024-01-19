using System;
using System.Linq.Expressions;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Repository
{
	public class CouponRepository : ICouponRepository
	{
        private readonly AppDbContext _dbContext;

        public CouponRepository(AppDbContext dbContext)
		{
            _dbContext = dbContext;
        }

        public async Task<Coupon> CreateAsync(Coupon entity)
        {
           await _dbContext.Coupons.AddAsync(entity);

            await _dbContext.SaveChangesAsync();

            return entity;

        }

        public async Task<Coupon?> DeleteAsync(int id)
        {
            var domain = await _dbContext.Coupons.FirstOrDefaultAsync(u => u.CouponId == id);

            if(domain != null)
            {
                _dbContext.Coupons.Remove(domain);
                await _dbContext.SaveChangesAsync();

                return domain;
            }

            return null;

        }

        public async Task<List<Coupon>> GetAllAsync(Expression<Func<Coupon, bool>> filter = null)
        {
            IQueryable<Coupon> query = _dbContext.Coupons;

            if(filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task<Coupon?> GetAsync(Expression<Func<Coupon, bool>> filter = null)
        {
            IQueryable<Coupon> query = _dbContext.Coupons;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Coupon?> UpdateAsync(int id, Coupon entity)
        {
            var existingModel = await _dbContext.Coupons.FirstOrDefaultAsync(u => u.CouponId == id);

            if(existingModel == null)
            {
                return null;
            }

            existingModel.CouponCode = entity.CouponCode;
            existingModel.DiscountAmount = entity.DiscountAmount;
            existingModel.MinAmount = entity.MinAmount;

            await _dbContext.SaveChangesAsync();

            return existingModel;

        }
    }
}

