using System;
using System.Collections.Generic;

namespace SlabProjectAPI.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public virtual List<ProjectTask> Tasks { get; set; }
    }
}