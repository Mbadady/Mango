using System;
using Mango.Services.AuthAPI.Models.DTO;

namespace Mango.Services.AuthAPI.Services.IService
{
	public interface IAuthService
	{

		Task<string> RegisterAsync(RegistrationRequestDTO registrationDTO);

		Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO);

		Task<bool> AssignRole(string email, string roleName);
	}
}

