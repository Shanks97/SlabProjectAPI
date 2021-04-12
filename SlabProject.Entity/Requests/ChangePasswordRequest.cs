using System.ComponentModel.DataAnnotations;

namespace SlabProject.Entity.Requests
{
    public class ChangePasswordRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}