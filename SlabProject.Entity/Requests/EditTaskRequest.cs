using System;

namespace SlabProject.Entity.Requests
{
    public class EditTaskRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? ExecutionDate { get; set; }
    }
}