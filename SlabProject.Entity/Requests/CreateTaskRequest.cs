using System;
using System.ComponentModel.DataAnnotations;

namespace SlabProject.Entity.Requests
{
    public record CreateTaskRequest
    (
        [Required]
        string Name,

        [Required]
        string Description,

        [Required]
        int ProjectId,

        [Required]
        DateTime ExecutionDate
    );
}