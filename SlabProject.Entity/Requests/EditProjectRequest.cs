using System;

namespace SlabProject.Entity.Requests
{
    public class EditProjectRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? FinishDate { get; set; }
    }
}