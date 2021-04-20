using SlabProjectAPI.Data;
using System.Threading.Tasks;

namespace SlabProject.Database.Repositories
{
    internal class TaskRepository : GenericRepository<Task>
    {
        public TaskRepository(ProjectDbContext projectContext) : base(projectContext)
        {
        }
    }
}