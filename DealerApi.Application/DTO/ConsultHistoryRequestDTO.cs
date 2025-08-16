using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealerApi.Application.DTO
{
    public class ConsultHistoryRequestDTO
    {
        public int ConsultHistoryId { get; set; }
        public string CustomerName { get; set; }
        public string CarName { get; set; }
        public string Note { get; set; }
        public decimal Budget { get; set; }
        public DateTime ConsultDate { get; set; }
        public string StatusConsultation { get; set; }
    }
}