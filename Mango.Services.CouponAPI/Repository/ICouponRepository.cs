using System;
using System.Linq.Expressions;
using Mango.Services.CouponAPI.Models;

namespace Mango.Services.CouponAPI.Repository
{
	public interface ICouponRepository
	{

		Task<List<Coupon>> GetAllAsync(Expression<Func<Coupon, bool>> filter = null);

		Task<Coupon?> GetAsync(Expression<Func<Coupon, bool>> filter = null);

		Task<Coupon> CreateAsync(Coupon entity);

		Task<Coupon?> UpdateAsync(int id, Coupon entity);

		Task<Coupon?> DeleteAsync(int id);

	}
}

