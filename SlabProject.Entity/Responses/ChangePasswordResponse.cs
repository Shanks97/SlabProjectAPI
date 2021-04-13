using System.Collections.Generic;

namespace SlabProject.Entity.Responses
{
    public record ChangePasswordResponse
    {
        public bool Success { get; init; }
        public List<string> NewPasswordErrors { get; init; }
        public List<string> OldPasswordErrors { get; init; }
        public List<string> Errors { get; init; }
    }
}