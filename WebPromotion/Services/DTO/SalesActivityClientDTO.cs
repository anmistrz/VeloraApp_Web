using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebPromotion.Services.DTO
{
    public class SalesActivityClientDTO
    {
        public int ActivityLogId { get; set; }

        public int CustomerId { get; set; }

        public int DealerId { get; set; }

        public int SalesPersonId { get; set; }

        public int? ConsultationId { get; set; }
        
        public int? TestDriveId { get; set; }

        public string ActivityType { get; set; }

        public int? NotificationId { get; set; }

        public DateTime ActivityDate { get; set; }

        public string Details { get; set; }

    }
}