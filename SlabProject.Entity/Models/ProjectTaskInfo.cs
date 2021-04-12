using System;

namespace SlabProject.Entity.Models
{
    public class ProjectTaskInfo
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ExecutionDate { get; set; }
    }
}