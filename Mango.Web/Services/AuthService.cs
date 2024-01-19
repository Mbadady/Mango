using System;
using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Mango.Web.Utility;

namespace Mango.Web.Services
{
	public class AuthService : IAuthService
	{
        private readonly IBaseService _baseService;

        public AuthService(IBaseService baseService)
		{
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> AssignRoleAsync(RegistrationRequestDTO requestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                Url = SD.AuthAPIBase + "/api/auth/assignRole",
                ApiType = SD.ApiType.POST,
                Data = requestDTO
            });
        }

        public async Task<ResponseDTO?> LoginAsync(LoginRequestDTO requestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                Url = SD.AuthAPIBase + "/api/auth/login",
                ApiType = SD.ApiType.POST,
                Data = requestDTO
            }, withBearer: false);
        }

        public async Task<ResponseDTO?> RegisterAsync(RegistrationRequestDTO requestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                Url = SD.AuthAPIBase + "/api/auth/register",
                ApiType = SD.ApiType.POST,
                Data = requestDTO
            }, withBearer: false);
        }
    }
}

