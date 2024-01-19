using System;
namespace Mango.Services.AuthAPI.Models.DTO
{
	public class ApiResponseDTO
	{
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
    }
}

