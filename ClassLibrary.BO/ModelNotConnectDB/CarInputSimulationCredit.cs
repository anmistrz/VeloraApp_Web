using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassLibrary.BO.ModelNotConnectDB
{
    public class CarInputSimulationCredit
    {
        public int DealerID { get; set; }
        public int CarID { get; set; }
        public double DownPayment { get; set; }
        public int TermMonths { get; set; }
        public float AnnualInterestRate { get; set; }
    }
}