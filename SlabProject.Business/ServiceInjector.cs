using Microsoft.Extensions.DependencyInjection;
using SlabProjectAPI.Services;
using SlabProjectAPI.Services.Interfaces;

namespace SlabProject.Business
{
    public static class ServiceInjector
    {
        public static void InjectServices(IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProjectService, ProjectService>();
        }
    }
}