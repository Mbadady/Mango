using System;
namespace Mango.Services.EmailAPI.Models.DTO
{
	public class CartDTO
	{
		public CartHeaderDTO CartHeaderDTO { get; set; }
		public IEnumerable<CartDetailsDTO>? CartDetailsDTO { get; set; }

	}
}

