using System;
using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.DTO;
using Mango.Services.AuthAPI.Services.IService;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Services
{
	public class AuthService : IAuthService
	{
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _db;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            AppDbContext db, IJwtTokenGenerator jwtTokenGenerator)
		{
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.Username.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            if(user == null || !isValid)
            {
                return new LoginResponseDTO() { User = null, Token = "" };
            }

            var roles = await _userManager.GetRolesAsync(user);
            // Generate Token
            var jwt = _jwtTokenGenerator.GenerateToken(user, roles);

            return new LoginResponseDTO()
            {
                User = new UserDTO() { Email = user.Email, Id = user.Id, Name = user.Name, PhoneNumber = user.PhoneNumber },
                Token = jwt
            };
        }

        public async Task<string> RegisterAsync(RegistrationRequestDTO registrationDTO)
        {
            var domainModel = new ApplicationUser()
            {
                Name = registrationDTO.Name,
                Email = registrationDTO.Email,
                PhoneNumber = registrationDTO.PhoneNumber,
                UserName = registrationDTO.Email,
                NormalizedEmail = registrationDTO.Email.ToUpper()

            };
            try
            {

                var user = await _userManager.CreateAsync(domainModel, registrationDTO.Password);

                if (user.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.FirstOrDefault(u => u.Email == registrationDTO.Email);

                    var userDTO = new UserDTO()
                    {
                        Id = userToReturn.Id,
                        Name = userToReturn.Name,
                        Email = userToReturn.Email,
                        PhoneNumber = userToReturn.PhoneNumber
                    };

                    return "";
                }

                else
                {
                    return user.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {
                return $"Error Encountered: {ex}"; 
            }
        }

        async Task<bool> IAuthService.AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }

                await _userManager.AddToRoleAsync(user, roleName);

                return true;
                
            }
            return false;
        }
    }
}

