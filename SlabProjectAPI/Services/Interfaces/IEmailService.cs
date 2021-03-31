using SlabProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlabProjectAPI.Services.Interfaces
{
    interface IEmailService
    {
        /// <summary>
        /// Send email to the new user
        /// </summary>
        /// <param name="email">New user email</param>
        /// <param name="password">New user password</param>
        void SendEmailUserCreated(string email, string password);

        /// <summary>
        /// Send email about the project that has been completed
        /// </summary>
        /// <param name="email"></param>
        /// <param name="project"></param>
        void SendEmailProjectCompleted(string email,  Project project);
    }
}
