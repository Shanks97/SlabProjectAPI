using System;

namespace SlabProject.Entity.Requests
{
    public record EditTaskRequest
    (
        string Name,
        string Description,
        DateTime? ExecutionDate
    );
}