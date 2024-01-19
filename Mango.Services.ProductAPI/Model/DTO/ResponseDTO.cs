using System;
namespace Mango.Services.ProductAPI.Model.DTO
{
	public class ResponseDTO
	{
		public object? Result { get; set; }
		public string Message { get; set; } = String.Empty;
		public bool IsSuccess { get; set; } = true;
	}
}

