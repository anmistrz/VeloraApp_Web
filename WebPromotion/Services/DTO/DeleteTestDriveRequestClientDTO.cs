using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebPromotion.Services.DTO
{
    public class DeleteTestDriveRequestClientDTO
    {
        public int DealerId { get; set; }
        public string Reason { get; set; }
        public int TestDriveId { get; set; }
        public int SalesPersonId { get; set; }
    }
}