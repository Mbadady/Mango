using System;
namespace Mango.Services.OrderAPI.Models.DTO
{
	public class CartDTO
	{
		public CartHeaderDTO CartHeaderDTO { get; set; }
		public IEnumerable<CartDetailsDTO>? CartDetailsDTO { get; set; }

	}
}

