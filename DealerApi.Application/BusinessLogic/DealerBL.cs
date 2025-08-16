using ClassLibrary.DAL.Interfaces;
using DealerApi.Application.DTO;
using DealerApi.Application.Interface;
using DealerApi.DAL;
using DealerApi.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerApi.Application
{
    public class DealerBL : IDealerBL
    {
        private readonly IDealer _dealerDAL;

        public DealerBL(IDealer dealerDAL)
        {
            _dealerDAL = dealerDAL;
        }

        public async Task<IEnumerable<DealerOptionsDTO>> GetDealerOptions()
        {
            try
            {
                var dealers = await _dealerDAL.GetAllAsync(); // Properly await the async call
                return dealers.Select(dealer => new DealerOptionsDTO
                {
                    DealerID = dealer.DealerId,
                    DealerName = dealer.DealerName
                });
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new Exception("Error retrieving dealer options", ex);
            }
        }
    }
}
