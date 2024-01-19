using System;
using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
	public interface IAuthService
	{

		Task<ResponseDTO?> RegisterAsync(RegistrationRequestDTO requestDTO);

		Task<ResponseDTO?> LoginAsync(LoginRequestDTO requestDTO);

		Task<ResponseDTO?> AssignRoleAsync(RegistrationRequestDTO requestDTO);
	}
}

