using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.Application.DTO;

namespace DealerApi.Application.Interface
{
    public interface IEmailNotificationBL
    {
        public Task<EmailNotificationDTO> SendEmail(string to, string subject, string body);
    }
}