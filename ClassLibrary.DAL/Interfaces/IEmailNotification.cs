using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.BO.ModelNotConnectDB;

namespace ClassLibrary.DAL.Interfaces
{
    public interface IEmailNotification
    {
        public Task<EmailNotification> SendEmailAsync(string to, string subject, string body);
    }
}