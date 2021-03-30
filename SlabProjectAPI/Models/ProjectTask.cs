using System;
using System.Collections.Generic;

namespace SlabProjectAPI.Models
{
    public class ProjectTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ExecutionDate { get; set; }
        public virtual List<Project> Projects { get; set; }
    }
}