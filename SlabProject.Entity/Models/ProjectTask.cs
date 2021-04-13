using System;

namespace SlabProject.Entity.Models
{
    public class ProjectTask
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime ExecutionDate { get; set; }
        public virtual Project Project { get; set; }
    }
}