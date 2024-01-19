using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mango.MessageBus;
using Mango.Services.AuthAPI.Models.DTO;
using Mango.Services.AuthAPI.Services.IService;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        protected ApiResponseDTO _apiResponse;

        public AuthController(IAuthService authService, IMessageBus messageBus, IConfiguration configuration)
        {
            _authService = authService;
            _messageBus = messageBus;
            _configuration = configuration;
            _apiResponse = new();
        }


        // POST api/values
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegistrationRequestDTO model)
        {
            var errorMessage = await _authService.RegisterAsync(model);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Message = errorMessage;

                return BadRequest(_apiResponse);
            }

            await _messageBus.PublishMessage(model.Email, _configuration.GetValue<string>("TopicAndQueueNames:RegisterQueue"), _configuration.GetValue<string>("RegisterUserConnectionString:ConnectionString"));
            _apiResponse.IsSuccess = true;
            _apiResponse.Message = errorMessage;
            _apiResponse.Result = null;

            return Ok(_apiResponse);
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginRequestDTO model)
        {

            var loginResponse = await _authService.LoginAsync(model);

            if(loginResponse.User == null)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Message = "Username or Password is incorrect";

                return BadRequest(_apiResponse);
            }

            _apiResponse.Result = loginResponse;

            return Ok(_apiResponse);
        }

        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDTO model)
        {
            var assignRoleSuccessful = await _authService.AssignRole(model.Email, model.RoleName);

            if (!assignRoleSuccessful)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Message = "Error Encountered";

                return BadRequest(_apiResponse);
            }

            return Ok(_apiResponse);
        }


    }
}

