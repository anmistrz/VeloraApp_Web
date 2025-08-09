using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.DAL.Interfaces;
using DealerApi.Entities.Models;

namespace ClassLibrary.DAL.Interfaces
{
    public interface IDealerCarUnit : ICrud<DealerCarUnit>
    {
        public Task<int> DeleteDealerCarUnitWithConsultHistoryAsync(int dealerCarUnitId);
    }
}