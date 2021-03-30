using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SlabProjectAPI.Data;
using SlabProjectAPI.Domain.Requests;
using SlabProjectAPI.Domain.Responses;
using SlabProjectAPI.Services;
using SlabProjectAPI.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace SlabProjectAPI.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleConstants.Operator)]
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

        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateProject([FromBody] CreateProjectRequest model)
        {
            if (ModelState.IsValid)
            {
                var result = _projectService.CreateProject(model);
                if (result.Success)
                    return CreatedAtAction(nameof(GetProjectById), new { id = result.Data.Id }, result);
                else
                    return BadRequest(result);
            }
            else
            {
                return BadRequest(new BaseRequestResponse<bool>()
                {
                    Errors = new System.Collections.Generic.List<string>()
                    {
                        "Invalid payload"
                    }
            });
            }
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateProjectTask([FromBody] CreateTaskRequest model)
        {
            if (ModelState.IsValid)
            {
                var result = _projectService.CreateTask(model);
                if (result.Success)
                    return CreatedAtAction(nameof(GetTasksById), new { id = result.Data.Id }, result);
                else
                    return BadRequest(result);
            }
            else
            {
                return BadRequest(new BaseRequestResponse<bool>()
                {
                    Errors = new System.Collections.Generic.List<string>()
                    {
                        "Invalid payload"
                    }
                });
            }
        }


        [HttpGet("[action]/{id}")]
        public IActionResult GetProjectById(int id)
        {
            var result = _projectService.GetProject(id);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetTasksById(int id)
        {
            var result = _projectService.GetTask(id);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("[action]")]
        public IActionResult GetProjects()
        {
            var result = _projectService.GetProjects();
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("[action]")]
        public IActionResult GetTasks()
        {
            var result = _projectService.GetTasks();
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }

    
}