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
                Console.WriteLine("Sending email...");
                Console.WriteLine($"To: {to}, Subject: {subject}, Body: {body}");
                Console.WriteLine($"SMTP Server: {_emailSettings.SmtpServer}, Port: {_emailSettings.Port}, Enable SSL: {_emailSettings.EnableSsl}");
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
                mailMessage.To.Add(to);

                Console.WriteLine($"MailMessage: From: {_emailSettings.SenderEmail}, To: {to}, Subject: {subject}");

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
                    Console.WriteLine("Error sending email: " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine("Inner exception: " + ex.InnerException.Message);
                // Handle exceptions (e.g., log them)
                throw new Exception("Error sending email", ex);
            }
        }
    }
}