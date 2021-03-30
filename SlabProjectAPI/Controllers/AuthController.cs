using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
using SlabProjectAPI.Services.Interfaces;
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
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IProjectService _projectService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(
            IAuthService authService,
            IProjectService projectService,
            RoleManager<IdentityRole> roleManager)
        {
            _authService = authService;
            _projectService = projectService;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Admin can register new users as Operator User
        /// </summary>
        /// <param name="user">The user's email</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest user)
        {
            await EnsureBasicRoles();

            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterUser(user);
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
        [Route("Login")]
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
        /// Admin can disable/enable the login for an Operator User by using the respective email
        /// </summary>
        /// <param name="email">Operator's email</param>
        /// <returns></returns>
        [HttpGet]
        [Route("SwitchOperatorStatus")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstants.Admin)]
        public async Task<IActionResult> SwitchEnableStatusForOperator(string email)
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