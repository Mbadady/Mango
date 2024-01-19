using System;
using Mango.Services.ShoppingCartAPI.Models.DTO;

namespace Mango.Services.ShoppingCartAPI.Services.IService
{
	public interface ICouponService
	{
		Task<CouponDTO> GetCouponAsync(string couponCode);
	}
}

