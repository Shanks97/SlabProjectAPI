using System;
using System.Collections.Generic;

namespace SlabProject.Entity.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public DateTime? FinishedDate { get; set; }
        public virtual List<ProjectTask> Tasks { get; set; }
    }
}