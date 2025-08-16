using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.DAL.Interfaces;
using DealerApi.Application.DTO;
using DealerApi.Application.Interface;

namespace DealerApi.Application
{
    public class DealerCarBL : IDealerCarBL
    {
        private readonly IDealerCar _dealerCarDAL;

        public DealerCarBL(IDealerCar dealerCarDAL)
        {
            _dealerCarDAL = dealerCarDAL;
        }


        public async Task<IEnumerable<DealerCarOptionsDTO>> GetOptionsDealerCarUnitByStatus(string status)
        {
            try
            {
                var dealerCarTuples = await _dealerCarDAL.GetOptionsDealerCarUnitByStatusAsync(status);

                if (dealerCarTuples == null || !dealerCarTuples.Any())
                {
                    return Enumerable.Empty<DealerCarOptionsDTO>();
                }

                var listDealerCarOptions = dealerCarTuples.ToList();

                return listDealerCarOptions.Select(tuple => new DealerCarOptionsDTO
                {
                    Dealers = new List<DealerOptionsDTO>
                    {
                        new DealerOptionsDTO
                        {
                            DealerID = tuple.Item1.DealerId,
                            DealerName = tuple.Item1.DealerName,
                            DealerCarUnitId = tuple.Item3.DealerCarUnitId,
                        }
                    },
                    Cars = new List<CarOptionsDTO>
                    {
                        new CarOptionsDTO
                        {
                            CarId = tuple.Item2.CarId,
                            CarName = tuple.Item2.CarModel,
                            DealerCarUnitId = tuple.Item3.DealerCarUnitId
                        }
                    },
                    DealerCarUnits = new List<DealerCarUnitOnlyIdDTO>
                    {
                        new DealerCarUnitOnlyIdDTO
                        {
                            DealerCarUnitId = tuple.Item3.DealerCarUnitId
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new Exception("Error retrieving dealer car options", ex);
            }
        }
    }
}