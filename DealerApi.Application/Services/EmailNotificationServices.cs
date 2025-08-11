using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.DAL.Interfaces;
using DealerApi.Application.DTO;
using DealerApi.Application.Interface;

namespace DealerApi.Application.Services
{
    public class EmailNotificationServices : IEmailNotificationServices
    {
        private readonly IEmailNotification _emailNotification;

        public EmailNotificationServices(IEmailNotification emailNotification)
        {
            _emailNotification = emailNotification;
        }

        public async Task<EmailNotificationDTO> SendEmail(string to, string subject, string body)
        {
            try
            {
                var emailNotification = await _emailNotification.SendEmailAsync(to, subject, body);
                return new EmailNotificationDTO
                {
                    ToEmail = emailNotification.ToEmail,
                    Subject = emailNotification.Subject,
                    Body = emailNotification.Body
                };
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log them)
                throw new Exception("Error sending email", ex);
            }
        }
    }
}