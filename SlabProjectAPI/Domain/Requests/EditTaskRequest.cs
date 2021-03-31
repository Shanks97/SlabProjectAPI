using System;
using System.ComponentModel.DataAnnotations;

namespace SlabProjectAPI.Domain.Requests
{
    public class EditTaskRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? ExecutionDate { get; set; }
    }
}