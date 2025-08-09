using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.DAL.Interfaces;
using DealerApi.Entities.Models;

namespace ClassLibrary.DAL.Interfaces
{
    public interface ISalesPerson : ICrud<SalesPerson>
    {
        public Task<IEnumerable<SalesPerson>> GetSalesPersonsByDealerIdAsync(int dealerId);
        public Task<IEnumerable<SalesPerson>> GetSalesPersonByEmailAsync(string email);
    }
}