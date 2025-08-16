using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.Application.DTO;
using DealerApi.DAL.Interfaces;
using DealerApi.Entities.Models;

namespace DealerApi.Application.Interface
{
    public interface ISalesPersonBL
    {
        Task<SalesPersonDTO> GetSalesPersonByIdAsync(int id);
        Task<IEnumerable<SalesPersonDTO>> GetAllSalesPersonsAsync();
        Task<SalesPersonDTO> CreateSalesPersonAsync(SalesPersonDTO salesPerson);
        Task<SalesPersonDTO> UpdateSalesPersonAsync(SalesPersonDTO salesPerson);
        Task<bool> DeleteSalesPersonAsync(int id);
        public Task<IEnumerable<SalesPersonDTO>> GetSalesPersonsByDealerIdAsync(int dealerId);
        public Task<IEnumerable<SalesPersonDTO>> GetSalesPersonByEmailAsync(string email);
    }
}