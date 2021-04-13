using System.ComponentModel.DataAnnotations;

namespace SlabProject.Entity.Requests
{
    public record UserRegistrationRequest
    (
        [Required]
        [EmailAddress]
        string UserName,

        [Required]
        string Password
    );
}