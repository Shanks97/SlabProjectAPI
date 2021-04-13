using System.ComponentModel.DataAnnotations;

namespace SlabProject.Entity.Requests
{
    public record UserLoginRequest
    (
        [Required]
        string Email,

        [Required]
        string Password
    );
}