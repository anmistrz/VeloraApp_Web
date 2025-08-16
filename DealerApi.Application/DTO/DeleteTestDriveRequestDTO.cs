using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealerApi.Application.DTO
{
    public class DeleteTestDriveRequestDTO
    {
        public int SalesPersonId { get; set; }
        public string Reason { get; set; }
        public int TestDriveId { get; set; }
        public int DealerId { get; set; }
        
    }
}