using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealerApi.Application.DTO
{
    public class DeleteConsultRequestDTO
    {
        public int DealerId { get; set; }
        public string Reason { get; set; }
        public int ConsultHistoryId { get; set; }
        public int SalesPersonId { get; set; }
    }
}