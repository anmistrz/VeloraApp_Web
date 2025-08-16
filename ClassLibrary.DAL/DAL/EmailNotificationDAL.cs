using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ClassLibrary.BO.ModelNotConnectDB;
using ClassLibrary.DAL.Interfaces;
using Microsoft.Extensions.Options;

namespace ClassLibrary.DAL.DAL
{
    public class EmailNotificationDAL : IEmailNotification
    {
        private readonly EmailSettings _emailSettings;

        public EmailNotificationDAL(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public async Task<EmailNotification> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var defaultTo = "anasardiansyah003@gmail.com";

                var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
                {
                    Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                    EnableSsl = _emailSettings.EnableSsl
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                // mailMessage.To.Add(to);

                //for email trying test
                mailMessage.To.Add(defaultTo);

                await smtpClient.SendMailAsync(mailMessage);
                return new EmailNotification
                {
                    ToEmail = to,
                    Subject = subject,
                    Body = "Success"
                };
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    // Handle exceptions (e.g., log them)
                    throw new Exception("Error sending email", ex);
                }
                // Optionally log the exception here
                return new EmailNotification
                {
                    ToEmail = to,
                    Subject = subject,
                    Body = $"Failed to send email: {ex.Message}"
                };
            }
        }
    }
}