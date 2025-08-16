using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealerApi.Application.DTO
{
    public class HandleRequestDTO
    {
        public string CustomerEmail { get; set; }
        public string NotificationType { get; set; }
        public string Message { get; set; }
        public string SalesPersonName { get; set; }
        public string DealerName { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}