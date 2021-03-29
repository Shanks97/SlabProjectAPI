using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SlabProjectAPI.Configuration;
using SlabProjectAPI.Constants;
using SlabProjectAPI.Data;
using SlabProjectAPI.Domain.Requests;
using SlabProjectAPI.Domain.Responses;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SlabProjectAPI.Controllers

{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;

        public AuthController(UserManager<IdentityUser> userManager, ProjectDbContext projectDbContext, IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            var l = projectDbContext.Tasks.AsNoTracking().ToList();
            var l2 = projectDbContext.Projects.AsNoTracking().ToList();
            var users = _userManager.Users.AsNoTracking().ToList();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto user, bool registerAsAdmin = false)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.UserName);

                if (existingUser != null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Email already exist"
                        }
                    });
                }

                var newUser = new IdentityUser() { Email = user.UserName, UserName = user.UserName };
                var isCreated = await _userManager.CreateAsync(newUser, user.Password);
                var isRoleCreated = await _userManager.AddToRoleAsync(newUser, registerAsAdmin ? RoleConstants.Admin : RoleConstants.Operator);
                if (isCreated.Succeeded && isRoleCreated.Succeeded)
                {
                    var jwtToken = GenerateJwtToken(newUser);

                    return Ok(new RegistrationResponse()
                    {
                        Result = true,
                        Token = jwtToken
                    });
                }

                return new JsonResult(new RegistrationResponse()
                {
                    Result = false,
                    Errors = isCreated.Errors.Select(x => x.Description).ToList()
                }
                        )
                { StatusCode = 500 };
            }

            return BadRequest(new RegistrationResponse()
            {
                Result = false,
                Errors = new List<string>()
                {
                    "Invalid state"
                }
            });
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            // We get our secret from the appsettings
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
