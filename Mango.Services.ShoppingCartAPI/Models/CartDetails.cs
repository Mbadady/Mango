using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mango.Services.ShoppingCartAPI.Models.DTO;

namespace Mango.Services.ShoppingCartAPI.Models
{
	public class CartDetails
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartDetailsId { get; set; }
		public int CartHeaderId { get; set; }
		[ForeignKey("CartHeaderId")]
		public CartHeader CartHeader { get; set; }
		public int ProductId { get; set; }

		[NotMapped]
		public ProductDTO ProductDTO { get; set; }
		public int Count { get; set; }

	}
}

