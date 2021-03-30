using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SlabProjectAPI.Data;
using SlabProjectAPI.Services;
using SlabProjectAPI.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace SlabProjectAPI.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly IProjectService _projectService;

        public ProjectController(
            ILogger<ProjectController> logger,
            IProjectService projectService
            )
        {
            _logger = logger;
            _projectService = projectService;
        }

        [HttpGet("GetProjects")]
        public IActionResult GetProjects()
        {
            var result = _projectService.GetProjects();
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }

    
}