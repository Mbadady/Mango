using System;
namespace Mango.Services.ProductAPI.Model.DTO
{
	public class UpdateProductDTO
	{
        //public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
    }
}

