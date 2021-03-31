using System.Collections.Generic;

namespace SlabProjectAPI.Domain.Responses
{
    public class ChangePasswordResponse
    {
        public bool Success { get; set; }
        public List<string> NewPasswordErrors { get; set; }
        public List<string> OldPasswordErrors { get; set; }
        public List<string> Errors { get; set; }
    }
}