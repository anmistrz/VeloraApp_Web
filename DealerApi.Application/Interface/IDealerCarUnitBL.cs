using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.Application.DTO;
using DealerApi.DAL.Interfaces;
using DealerApi.Entities.Models;

namespace DealerApi.Application.Interface
{
    public interface IDealerCarUnitBL
    {
        public Task<int> DeleteDealerCarUnitWithConsultHistory(int dealerCarUnitId);
        public Task<DealerCarUnitDTO> GetDealerCarUnitById(int id);
        public Task<IEnumerable<DealerCarUnitDTO>> GetAllDealerCarUnits();
        public Task<DealerCarUnitDTO> CreateDealerCarUnit(DealerCarUnitDTO dealerCarUnit);
        public Task<DealerCarUnitDTO> UpdateDealerCarUnit(DealerCarUnitDTO dealerCarUnit);
        public Task<bool> DeleteDealerCarUnit(int id);
    }
}