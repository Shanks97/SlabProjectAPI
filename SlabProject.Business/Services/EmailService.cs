using Microsoft.Extensions.Options;
using SlabProject.Entity.Models;
using SlabProjectAPI.Configuration;
using SlabProjectAPI.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace SlabProjectAPI.Services
{
    public class EmailService : IEmailService
    {
        private MailConfig _mailSettings;

        public EmailService(IOptionsMonitor<MailConfig> mailMonitor)
        {
            _mailSettings = mailMonitor.CurrentValue;
        }

        public void SendEmailUserCreated(string email, string password)
        {
            var fromAddress = new MailAddress(_mailSettings.Mail, _mailSettings.DisplayName);
            var toAddress = new MailAddress(email, email);
            string subject = "User Created";
            string body = $"your email is: {email} & your password is: {password}";
            SendEmail(fromAddress, toAddress, subject, body);
        }

        public void SendEmailProjectCompleted(string email, Project project)
        {
            var fromAddress = new MailAddress(_mailSettings.Mail, _mailSettings.DisplayName);
            var toAddress = new MailAddress(email, email);
            string subject = "Project Completed";
            string body = $"The Project {project.Name} with id: {project.Id} has been completed, congratz!";
            SendEmail(fromAddress, toAddress, subject, body);
        }

        private void SendEmail(MailAddress from, MailAddress to, string subject, string body)
        {
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from.Address, _mailSettings.Password)
            };
            using (var message = new MailMessage(from, to)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}