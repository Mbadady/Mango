using System;
namespace Mango.Web.Models
{
	public class CartDTO
	{
		public CartHeaderDTO CartHeaderDTO { get; set; }
		public IEnumerable<CartDetailsDTO>? CartDetailsDTO { get; set; }

	}
}

