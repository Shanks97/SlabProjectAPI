using SlabProject.Entity;
using SlabProject.Entity.Models;
using SlabProject.Entity.Requests;
using SlabProject.Entity.Responses;
using System.Threading.Tasks;

namespace SlabProjectAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterUser(UserRegistrationRequest request, bool sendEmail = true, bool isAdmin = false);

        Task<AuthResult> Login(UserLoginRequest request);

        Task<BaseRequestResponse<User>> SwitchOperatorAuthentication(string email);

        Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest changePasswordRequest);
    }
}