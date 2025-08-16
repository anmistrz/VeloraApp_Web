using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.Application.DTO;

namespace DealerApi.Application.Interface
{
    public interface IDealerCarBL
    {
        public Task<IEnumerable<DealerCarOptionsDTO>> GetOptionsDealerCarUnitByStatus(string status);
    }
}