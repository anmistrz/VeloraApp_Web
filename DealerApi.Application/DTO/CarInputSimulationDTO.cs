using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealerApi.Application.DTO
{
    public class CarInputSimulationDTO
    {
        public int DealerID { get; set; }
        public int CarID { get; set; }
        public double DownPayment { get; set; }
        public int TermMonths { get; set; }
        public float AnnualInterestRate { get; set; }
    }
}