using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.BO.ModelNotConnectDB;
using ClassLibrary.DAL.Interfaces;
using DealerApi.Application.DTO;
using DealerApi.Application.Interface;

namespace DealerApi.Application.BusinessLogic
{
    public class SimulationCreditBL : ICarSimulationBL
    {
        private readonly ISimulationCredit _simulationCreditDAL;

        public SimulationCreditBL(ISimulationCredit simulationCreditDAL)
        {
            _simulationCreditDAL = simulationCreditDAL;
        }

        public async Task<IEnumerable<CarSimulationCreditDTO>> GetCarSimulationCreditsAsync(int dealerId, int carId, double downPayment, int termMonths, float annualInterestRate)
        {

            try
            {
                var convertDTO = new CarInputSimulationCredit
                {
                    DealerID = dealerId,
                    CarID = carId,
                    DownPayment = downPayment,
                    TermMonths = termMonths,
                    AnnualInterestRate = annualInterestRate
                };
                var result = await _simulationCreditDAL.GetCarSimulationCreditsAsync(convertDTO);
                return result.Select(x => new CarSimulationCreditDTO
                {
                    DealerCarID = x.DealerCarID,
                    DealerPrice = x.DealerPrice,
                    DealerName = x.DealerName,
                    CarModel = x.CarModel,
                    Tax = x.Tax,
                    PriceAfterTax = x.PriceAfterTax,
                    DownPayment = x.DownPayment,
                    LoanAmount = x.LoanAmount,
                    TermMonths = x.TermMonths,
                    AnnualInterestRate = x.AnnualInterestRate,
                    MonthlyPrincipal = x.MonthlyPrincipal,
                    TotalInterest = x.TotalInterest,
                    MonthlyInterest = x.MonthlyInterest,
                    MonthlyPayment = x.MonthlyPayment
                });
            }
            catch (Exception ex)
            {
                // Handle exceptions
                throw new InvalidOperationException("Error retrieving car simulation credits", ex);
            }
        }
    }
}