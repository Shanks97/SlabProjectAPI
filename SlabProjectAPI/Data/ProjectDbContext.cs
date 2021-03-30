using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SlabProjectAPI.Models;

namespace SlabProjectAPI.Data
{
    public class ProjectDbContext : IdentityDbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }
        public DbSet<User> UsersInformation { get; set; }

        public ProjectDbContext(DbContextOptions<ProjectDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<Project>().HasMany(x => x.Tasks).WithOne(x => x.Project);
        }
    }
}