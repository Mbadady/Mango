using System;
using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Mango.Web.Utility;

namespace Mango.Web.Services
{
	public class CouponService : ICouponService
	{
        private readonly IBaseService _baseService;

        public CouponService(IBaseService baseService)
		{
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> CreateCouponAsync(CouponDTO couponDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                Url = SD.CouponAPIBase + "/api/coupon",
                ApiType = SD.ApiType.POST,
                Data = couponDTO
            });
        }

        public async Task<ResponseDTO?> DeleteCouponAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                Url = SD.CouponAPIBase + "/api/coupon/" + id,
                ApiType = SD.ApiType.DELETE
            });
        }

        public async Task<ResponseDTO?> GetAllCouponAsync()
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                Url = SD.CouponAPIBase + "/api/coupon",
                ApiType = SD.ApiType.GET
            });
        }

        public async Task<ResponseDTO?> GetCouponByCodeAsync(string couponCode)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                Url = SD.CouponAPIBase + "/api/coupon/" + couponCode,
                ApiType = SD.ApiType.GET
            });
        }

        public async Task<ResponseDTO?> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                Url = SD.CouponAPIBase + "/api/coupon/" + id,
                ApiType = SD.ApiType.GET
            });
        }

        public async Task<ResponseDTO?> UpdateCouponAsync(CouponDTO couponDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                Url = SD.CouponAPIBase + "/api/coupon/" + couponDTO.CouponId,
                ApiType = SD.ApiType.PUT,
                Data = couponDTO
            });
        }
    }
}

