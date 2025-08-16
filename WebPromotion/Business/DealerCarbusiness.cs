using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPromotion.Services.DTO;
using WebPromotion.Services.DealerCar;

namespace WebPromotion.Business
{
    public class DealerCarbusiness : IDealerCarBusiness
    {
        private IDealerCarServices _dealerCarServices;

        public DealerCarbusiness(IDealerCarServices dealerCarServices)
        {
            _dealerCarServices = dealerCarServices;
        }

        public IEnumerable<List<DealerCarUnitOptionsDTO>> GetOptionsDealerCarUnitByStatus(string status)
        {
            // This is a placeholder for the actual implementation
            var dtos = _dealerCarServices.GetOptionsDealerCarUnitByStatusAsync(status).Result;

            var result = dtos.Select(dto => new List<DealerCarUnitOptionsDTO>
            {
                new DealerCarUnitOptionsDTO
                {
                    Dealers = dto.Dealers.Select(d => new DealerOptionsDTO
                    {
                        DealerID = d.DealerID,
                        DealerName = d.DealerName,
                        DealerCarUnitId = d.DealerCarUnitId
                    }).ToList(),
                    Cars = dto.Cars.Select(c => new CarOptionsDTO
                    {
                        CarId = c.CarId,
                        CarName = c.CarName,
                        DealerCarUnitId = c.DealerCarUnitId
                    }).ToList(),
                    DealerCarUnits = dto.DealerCarUnits.Select(u => new DealerCarUnitDTO
                    {
                        DealerCarUnitId = u.DealerCarUnitId
                    }).ToList()
                }
            }).ToList();
            
            return result;
        }


    }
}