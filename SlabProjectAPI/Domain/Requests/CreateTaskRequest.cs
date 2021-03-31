using System;
using System.ComponentModel.DataAnnotations;

namespace SlabProjectAPI.Domain.Requests
{
    public class CreateTaskRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public DateTime ExecutionDate { get; set; }
    }
}