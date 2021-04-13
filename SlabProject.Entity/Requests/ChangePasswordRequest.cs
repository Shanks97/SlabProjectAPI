using System.ComponentModel.DataAnnotations;

namespace SlabProject.Entity.Requests
{
    public record ChangePasswordRequest
    (
        [Required]
        string Email,

        [Required]
        string OldPassword,

        [Required]
        string NewPassword
    );
}