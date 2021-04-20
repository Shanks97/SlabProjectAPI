using SlabProject.Entity.Models;
using SlabProjectAPI.Data;

namespace SlabProject.Database.Repositories
{
    public class ProjectRepository : GenericRepository<Project>
    {
        public ProjectRepository(ProjectDbContext projectContext) : base(projectContext)
        {
        }
    }
}