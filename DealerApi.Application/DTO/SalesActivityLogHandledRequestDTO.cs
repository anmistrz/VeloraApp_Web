using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealerApi.Application.DTO
{
    public class SalesActivityLogHandledRequestDTO
    {
        public int SalesActivityLogId { get; set; }
        public string CustomerName { get; set; }
        public string CarName { get; set; }
        public string ActivityType { get; set; }
        public DateTime ActivityDate { get; set; }
        public string Details { get; set; }
        public int? ConsultationId { get; set; }
        public int? TestDriveId { get; set; }
    }
}