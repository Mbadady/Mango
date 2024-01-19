using System;
using System.Text.Json;
using Mango.Services.ShoppingCartAPI.Models.DTO;
using Mango.Services.ShoppingCartAPI.Services.IService;

namespace Mango.Services.ShoppingCartAPI.Services
{
	public class CouponService : ICouponService
	{
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory)
		{
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CouponDTO> GetCouponAsync(string couponCode)
        {
            var client = _httpClientFactory.CreateClient("Coupon");

            var response = await client.GetAsync($"/api/coupon/GetByCode/{couponCode}");

            var resp = await response.Content.ReadFromJsonAsync<ResponseDTO>();

            if (resp != null && resp.IsSuccess)
            {
                return JsonSerializer.Deserialize<CouponDTO>(Convert.ToString(resp.Result));
            }

            return new CouponDTO();
        }
    }
}

