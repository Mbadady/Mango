using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.CouponAPI.Models
{
	public class Coupon
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CouponId { get; set; }

		[Required]
		public string CouponCode { get; set; }

		[Required]
		public double DiscountAmount { get; set; }
		public int MinAmount { get; set; }
	}
}

