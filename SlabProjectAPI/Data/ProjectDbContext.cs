using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SlabProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
