using SlabProjectAPI.Domain;
using SlabProjectAPI.Domain.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlabProjectAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterUser(UserRegistrationRequest request, bool registerAsAdmin = false);
        Task<AuthResult> Login(UserLoginRequest request);
    }
}
