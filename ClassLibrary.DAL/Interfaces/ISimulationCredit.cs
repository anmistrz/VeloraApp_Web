using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.BO.ModelNotConnectDB;

namespace ClassLibrary.DAL.Interfaces
{
    public interface ISimulationCredit
    {
        public Task<IEnumerable<CarSimulationCredit>> GetCarSimulationCreditsAsync(CarInputSimulationCredit carInputSimulationCredit);
    }
}