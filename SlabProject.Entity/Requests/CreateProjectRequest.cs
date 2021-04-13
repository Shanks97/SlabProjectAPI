using System;
using System.ComponentModel.DataAnnotations;

namespace SlabProject.Entity.Requests
{
    public record CreateProjectRequest
    (
        [Required]
        string Name,

        [Required]
        string Description,

        [Required]
        DateTime StartDate,

        [Required]
        DateTime FinishDate
    );
}