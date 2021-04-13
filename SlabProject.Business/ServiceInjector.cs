using Microsoft.Extensions.DependencyInjection;
using SlabProjectAPI.Data;
using SlabProjectAPI.Services;
using SlabProjectAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
