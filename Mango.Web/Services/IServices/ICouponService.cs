using System;
using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
	public interface ICouponService
	{
		Task<ResponseDTO?> GetCouponByCodeAsync(string couponCode);

        Task<ResponseDTO?> GetCouponByIdAsync(int id);

        Task<ResponseDTO?> GetAllCouponAsync();

        Task<ResponseDTO?> CreateCouponAsync(CouponDTO couponDTO);

        Task<ResponseDTO?> UpdateCouponAsync(CouponDTO couponDTO);

        Task<ResponseDTO?> DeleteCouponAsync(int id);
    }
}

