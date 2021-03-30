using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SlabProjectAPI.Configuration;
using SlabProjectAPI.Constants;
using SlabProjectAPI.Domain;
using SlabProjectAPI.Domain.Requests;
using SlabProjectAPI.Domain.Responses;
using SlabProjectAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SlabProjectAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtConfig _jwtConfig;

        public AuthService(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        public async Task<AuthResult> Login(UserLoginRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser == null)
            {
                return new RegistrationResponse()
                {
                    Result = false,
                    Errors = new List<string>()
                        {
                            "Email doesn't exist"
                        }
                };
            }

            var isCorrect = await _userManager.CheckPasswordAsync(existingUser, request.Password);

            if (isCorrect)
            {
                var jwtToken = GenerateJwtToken(existingUser);

                return new RegistrationResponse()
                {
                    Result = true,
                    Token = jwtToken
                };
            }
            else
            {
                return new RegistrationResponse()
                {
                    Result = false,
                    Errors = new List<string>()
                        {
                            "Invalid authentication request"
                        }
                };
            }
        }

        public async Task<AuthResult> RegisterUser(UserRegistrationRequest request, bool registerAsAdmin = false)
        {
            //EMAIL VALIDATOR
            var existingUser = await _userManager.FindByEmailAsync(request.UserName);

            if (existingUser != null)
            {
                return new RegistrationResponse()
                {
                    Result = false,
                    Errors = new List<string>()
                        {
                            "Email already exist"
                        }
                };
            }

            //PASSWORD VALIDATOR
            var passErrors = new List<string>();
            var validators = _userManager.PasswordValidators;
            foreach (var validator in validators)
            {
                var isValid = await validator.ValidateAsync(_userManager, null, request.Password);
                if (!isValid.Succeeded)
                    passErrors.AddRange(isValid.Errors.Select(x => x.Description));
            }

            if (passErrors.Any())
            {
                return new RegistrationResponse()
                {
                    Result = false,
                    Errors = passErrors
                };
            }

            var newUser = new IdentityUser() { Email = request.UserName, UserName = request.UserName };
            var isCreated = await _userManager.CreateAsync(newUser, request.Password);
            var user1 = await _userManager.FindByEmailAsync(newUser.Email);

            if (isCreated.Succeeded)
            {
                await _userManager.AddToRoleAsync(user1, registerAsAdmin ? RoleConstants.Admin : RoleConstants.Operator);

                var jwtToken = GenerateJwtToken(newUser);
                return new RegistrationResponse()
                {
                    Result = true,
                    Token = jwtToken
                };
            }

            return new RegistrationResponse()
            {
                Result = false,
                Errors = isCreated.Errors.Select(x => x.Description).ToList()
            };
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                // the JTI is used for refresh token
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }

    }
}
