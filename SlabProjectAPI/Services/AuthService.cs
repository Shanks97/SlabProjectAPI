using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SlabProjectAPI.Configuration;
using SlabProjectAPI.Constants;
using SlabProjectAPI.Data;
using SlabProjectAPI.Domain;
using SlabProjectAPI.Domain.Requests;
using SlabProjectAPI.Domain.Responses;
using SlabProjectAPI.Models;
using SlabProjectAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SlabProjectAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly ProjectDbContext _projectDbContext;
        private readonly JwtConfig _jwtConfig;
        private readonly MailConfig _mailSettings;

        public AuthService(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ProjectDbContext projectDbContext,
            IEmailService emailService,
            IOptionsMonitor<JwtConfig> optionsMonitor,
            IOptionsMonitor<MailConfig> mailMonitor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _projectDbContext = projectDbContext;
            _jwtConfig = optionsMonitor.CurrentValue;
            _mailSettings = mailMonitor.CurrentValue;
        }

        public async Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest changePasswordRequest)
        {
            var result1 = await ValidatePassword(changePasswordRequest.OldPassword);
            var result2 = await ValidatePassword(changePasswordRequest.NewPassword);
            if (result1.Any() || result2.Any())
            {
                return new ChangePasswordResponse()
                {
                    OldPasswordErrors = result1,
                    NewPasswordErrors = result2,
                    Success = false
                };
            }

            var user = await _userManager.FindByEmailAsync(changePasswordRequest.Email);
            if (user == null)
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
                            "User doesn't exist"
                        }
                };
            }
            var roles = await _userManager.GetRolesAsync(existingUser);
            var userInfo = _projectDbContext.UsersInformation.AsNoTracking().FirstOrDefault(x => x.UserName == request.Email);
            if (!userInfo.Enabled)
            {
                return new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Unauthorized to Login, your login is disabled"
                    }
                };
            }

            var isCorrect = await _userManager.CheckPasswordAsync(existingUser, request.Password);

            if (isCorrect)
            {
                var jwtToken = await GenerateJwtToken(existingUser);

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
                await _userManager.AddToRoleAsync(user1, RoleConstants.Admin);
                _projectDbContext.UsersInformation.Add(new Models.User
                {
                    Enabled = true,
                    UserName = request.UserName
                });
                _projectDbContext.SaveChanges();
                var jwtToken = await GenerateJwtToken(newUser);
                _emailService.SendEmailUserCreated(request.UserName, request.Password);
                return new RegistrationResponse()
                {
                    Result = true,
                    Token = jwtToken
                };
            }

            return new RegistrationResponse()
            {
                Errors = isCreated.Errors.Select(x => x.Description).ToList()
            };
        }

        public async Task<BaseRequestResponse<User>> SwitchOperatorAuthentication(string email)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                var isOperator = await _userManager.IsInRoleAsync(existingUser, RoleConstants.Operator);
                if (isOperator)
                {
                    var info = _projectDbContext.UsersInformation.FirstOrDefault(x => x.UserName == email);
                    info.Enabled = !info.Enabled;
                    _projectDbContext.UsersInformation.Update(info);
                    _projectDbContext.SaveChanges();
                    return new BaseRequestResponse<User>()
                    {
                        Data = info,
                        Success = true
                    };
                }
                else
                {
                    return new BaseRequestResponse<User>()
                    {
                        Errors = new List<string>()
                        {
                            "User is not an operator"
                        }
                    };
                }
            }
            return new BaseRequestResponse<User>()
            {
                Errors = new List<string>()
                {
                    "email doesn't exist"
                }
            };
        }

        private async Task<string> GenerateJwtToken(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            // Get User roles and add them to claims
            var roles = await _userManager.GetRolesAsync(user);
            AddRolesToClaims(claims, roles);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(1);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void AddRolesToClaims(List<Claim> claims, IEnumerable<string> roles)
        {
            foreach (var role in roles)
            {
                var roleClaim = new Claim(ClaimTypes.Role, role);
                claims.Add(roleClaim);
            }
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