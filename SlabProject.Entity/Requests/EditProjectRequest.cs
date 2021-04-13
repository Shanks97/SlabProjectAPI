using System;

namespace SlabProject.Entity.Requests
{
    public record EditProjectRequest
    (
        string Name,
        string Description,
        DateTime? FinishDate
    );
}