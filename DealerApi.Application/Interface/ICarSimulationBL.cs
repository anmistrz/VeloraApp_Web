using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.Application.DTO;

namespace DealerApi.Application.Interface
{
    public interface ICarSimulationBL
    {
        Task<IEnumerable<CarSimulationCreditDTO>> GetCarSimulationCreditsAsync(int dealerCarId, int carId, double downPayment, int termMonths, float annualInterestRate);
    }
}