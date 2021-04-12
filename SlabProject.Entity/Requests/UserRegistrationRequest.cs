using System.ComponentModel.DataAnnotations;

namespace SlabProject.Entity.Requests
{
    public class UserRegistrationRequest
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}