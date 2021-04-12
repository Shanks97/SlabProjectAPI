using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SlabProject.Entity.Constants;
using SlabProject.Entity.Requests;
using SlabProject.Entity.Responses;
using SlabProjectAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using SlabProject.Entity;

namespace SlabProjectAPI.Controllers

{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(
            IAuthService authService,
            RoleManager<IdentityRole> roleManager)
        {
            _authService = authService;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Admin can register new users as Operator User
        /// </summary>
        /// <param name="user">The user's email</param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest user)
        {
            await EnsureBasicRoles();

            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterUser(user);
                if (result.Result)
                    return Created(nameof(Login), result);
                else
                    return BadRequest(result);
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

        /// <summary>
        /// Anonymous User Login
        /// </summary>
        /// <param name="user">Model to request the Login</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.Login(user);
                if (result.Result)
                    return Ok(result);
                else
                    return BadRequest(result);
            }

            return BadRequest(new RegistrationResponse()
            {
                Result = false,
                Errors = new List<string>()
                {
                    "Invalid payload"
                }
            });
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="changePasswordRequest">model to request a new password</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("changePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SwitchEnableStatusForOperator([FromBody] ChangePasswordRequest changePasswordRequest)
        {
            var result = await _authService.ChangePassword(changePasswordRequest);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        /// <summary>
        /// Admin can disable/enable the login for an Operator User by using the respective email
        /// </summary>
        /// <param name="email">Operator's email</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("switchOperatorStatus")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstants.Admin)]
        public async Task<IActionResult> SwitchEnableStatusForOperator([FromQuery] string email)
        {
            var result = await _authService.SwitchOperatorAuthentication(email);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        private async Task EnsureBasicRoles()
        {
            var roles = new List<string>() { RoleConstants.Admin, RoleConstants.Operator };

            foreach (var roleName in roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
                }
            }
        }
    }
}