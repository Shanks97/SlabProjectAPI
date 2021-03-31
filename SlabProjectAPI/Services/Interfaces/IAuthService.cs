using SlabProjectAPI.Domain;
using SlabProjectAPI.Domain.Requests;
using SlabProjectAPI.Domain.Responses;
using SlabProjectAPI.Models;
using System.Threading.Tasks;

namespace SlabProjectAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterUser(UserRegistrationRequest request);

        Task<AuthResult> Login(UserLoginRequest request);

        Task<BaseRequestResponse<User>> SwitchOperatorAuthentication(string email);

        Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest changePasswordRequest);
    }
}