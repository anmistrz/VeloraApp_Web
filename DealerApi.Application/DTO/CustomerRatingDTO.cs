using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealerApi.Application.DTO
{
    public class CustomerRatingDTO
    {
        public int CustomerId { get; set; }

        public int? SalesPersonId { get; set; }

        public int? DealerId { get; set; }

        public int? ConsultHistoryId { get; set; }

        public int? TestDriveId { get; set; }

        public string RatingType { get; set; }

        public double RatingValue { get; set; }

        public string Comments { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; }

    }
}