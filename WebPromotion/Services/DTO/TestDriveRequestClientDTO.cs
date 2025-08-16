using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebPromotion.Services.DTO
{
    public class TestDriveRequestClientDTO
    {
        public int TestDriveId { get; set; }
        public string CustomerName { get; set; }
        public string CarName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
    }
}