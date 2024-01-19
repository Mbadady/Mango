using System;
namespace Mango.Services.OrderAPI.Models.DTO
{
	public class CouponDTO
	{
        public int CouponId { get; set; }
        public string CouponCode { get; set; }
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}

