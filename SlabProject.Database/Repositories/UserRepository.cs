using SlabProject.Entity.Models;
using SlabProjectAPI.Data;

namespace SlabProject.Database.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(ProjectDbContext projectContext) : base(projectContext)
        {
        }
    }
}