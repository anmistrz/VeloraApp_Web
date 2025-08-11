using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebPromotion.Services.DTO
{
    public class NotificationDTO
    {
        public int NotificationId { get; set; }
        public int? CustomerId { get; set; }

        public int? DealerId { get; set; }

        public int? SalesPersonId { get; set; }

        public int? ConsultHistoryId { get; set; }

        public int? TestDriveId { get; set; }

        public string NotificationType { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; }

        public int Priority { get; set; } 
        
         public DateTime CreatedAt { get; set; }
    }
}