using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SlabProjectAPI.Domain.Requests
{
    public class UserRegistrationRequestDto
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
