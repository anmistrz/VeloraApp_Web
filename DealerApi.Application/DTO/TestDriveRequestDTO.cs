using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealerApi.Application.DTO
{
    public class TestDriveRequestDTO
    {
        public int TestDriveId { get; set; }
        public string CustomerName { get; set; }
        public string CarName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
    }
}