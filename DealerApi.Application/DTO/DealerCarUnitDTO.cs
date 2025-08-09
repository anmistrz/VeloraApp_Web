using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealerApi.Application.DTO
{
    public class DealerCarUnitDTO
    {
        public int DealerCarUnitId { get; set; }

        public int DealerCarId { get; set; }

        public string Vin { get; set; }

        public string Color { get; set; }

        public int? YearManufacture { get; set; }

        public string Status { get; set; }

        public DateTime AddedDate { get; set; }
    }
}