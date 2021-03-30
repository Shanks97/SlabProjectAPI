using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SlabProjectAPI.Configuration;
using SlabProjectAPI.Constants;
using SlabProjectAPI.Data;
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
        private readonly ProjectDbContext _projectDbContext;
        private readonly JwtConfig _jwtConfig;

        public AuthService(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ProjectDbContext projectDbContext,
            IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _projectDbContext = projectDbContext;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        public async Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest changePasswordRequest)
        {
            var result1 = await ValidatePassword(changePasswordRequest.OldPassword);
            var result2 = await ValidatePassword(changePasswordRequest.NewPassword);
            if(result1.Any() || result2.Any())
            {
                return new ChangePasswordResponse()
                {
                    OldPasswordErrors = result1,
                    NewPasswordErrors = result2,
                    Success = false
                };
            }

            var user = await _userManager.FindByEmailAsync(changePasswordRequest.Email);
            if(user == null)
            {
                return new ChangePasswordResponse()
                {
                    Errors = new List<string>()
                    {
                        "email doesn't exist"
                    },
                    Success = false
                };
            }

            var passwordChanged = await _userManager.ChangePasswordAsync(user, changePasswordRequest.OldPassword, changePasswordRequest.NewPassword);
            if (passwordChanged.Succeeded)
            {
                return new ChangePasswordResponse()
                {
                    Success = true
                };
            }
            else
            {
                return new ChangePasswordResponse()
                {
                    Success = false,
                    Errors = passwordChanged.Errors.Select(x => x.Description).ToList()
                };
            }
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
                var userInfo = _projectDbContext.UsersInformation.FirstOrDefault(x => x.UserName == request.Email);
                if (userInfo == null)
                {
                    return new RegistrationResponse()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Auth for this email is disabled"
                        }
                    };
                }
                else
                {
                    var jwtToken = GenerateJwtToken(existingUser);

                    return new RegistrationResponse()
                    {
                        Result = true,
                        Token = jwtToken
                    };
                }
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

        public async Task<AuthResult> RegisterUser(UserRegistrationRequest request)
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
            var passErrors = await ValidatePassword(request.Password);

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
                await _userManager.AddToRoleAsync(user1, RoleConstants.Operator);
                _projectDbContext.UsersInformation.Add(new Models.User
                {
                    Enabled = true,
                    UserName = request.UserName
                });
                _projectDbContext.SaveChanges();
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

        public async Task<BaseRequestResponse<bool>> SwitchOperatorAuthentication(string email)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if(existingUser != null)
            {
                var isOperator = await _userManager.IsInRoleAsync(existingUser, RoleConstants.Operator);
                if (isOperator)
                {
                    var info = _projectDbContext.UsersInformation.FirstOrDefault(x => x.UserName == email);
                    if (info.Enabled)
                    {
                        return new BaseRequestResponse<bool>()
                        {
                            Data = true,
                            Success = true
                        };
                    }
                    else
                    {
                        return new BaseRequestResponse<bool>()
                        {
                            Data = false,
                            Success = false
                        };
                    }
                }
            }
            return new BaseRequestResponse<bool>()
            {
                Data = false,
                Success = false,
                Errors = new List<string>()
                {
                    "email doesn't exist"
                }
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

        private async Task<List<string>> ValidatePassword(string password)
        {
            var validators = _userManager.PasswordValidators;
            var passErrors = new List<string>();
            foreach (var validator in validators)
            {
                var isValid = await validator.ValidateAsync(_userManager, null, password);
                if (!isValid.Succeeded)
                    passErrors.AddRange(isValid.Errors.Select(x => x.Description));
            }
            return passErrors;
        }

    }
}
