using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealerApi.Application.DTO
{
    public class CarSimulationCreditDTO
    {
        public int DealerCarID { get; set; }
        public double DealerPrice { get; set; }
        public string DealerName { get; set; }
        public string CarModel { get; set; }
        public double Tax { get; set; }
        public double PriceAfterTax { get; set; }
        public double DownPayment { get; set; }
        public double LoanAmount { get; set; }
        public int TermMonths { get; set; }
        public float AnnualInterestRate { get; set; }
        public double MonthlyPrincipal { get; set; }
        public double TotalInterest { get; set; }
        public double MonthlyInterest { get; set; }
        public double MonthlyPayment { get; set; }
    }
}