using System.Collections.Generic;

namespace SlabProject.Entity
{
    public record AuthResult
    {
        public string Token { get; init; }
        public bool Result { get; init; }
        public List<string> Errors { get; init; }
    }
}