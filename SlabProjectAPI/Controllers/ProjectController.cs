using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SlabProjectAPI.Data;
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
        private readonly ProjectDbContext _projectDbContext;

        public ProjectController(
            ILogger<ProjectController> logger,
            ProjectDbContext projectDbContext
            )
        {
            _logger = logger;
            _projectDbContext = projectDbContext;
        }

        [HttpGet]
        public IActionResult GetProjects()
        {
            return Ok(_projectDbContext.Projects.AsNoTracking().ToList());
        }
    }

    
}