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
    [AllowAnonymous]
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

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest user, bool registerAsAdmin = false)
        {
            await EnsureBasicRoles();

            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterUser(user, registerAsAdmin);
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

        [HttpPost]
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