using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.DAL.Interfaces;
using DealerApi.Application.DTO;
using DealerApi.Application.Interface;

namespace DealerApi.Application
{
    public class CarBL : ICarBL
    {
        private readonly ICar _carDAL;

        public CarBL(ICar carDAL)
        {
            _carDAL = carDAL;
        }

        public async Task<IEnumerable<CarOptionsDTO>> GetCarOptions()
        {
            try 
            {
                var cars = await _carDAL.GetAllAsync(); // Properly await the async call
                return cars.Select(car => new CarOptionsDTO
                {
                    CarId = car.CarId,
                    CarName = car.CarModel
                });
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new Exception("Error retrieving car options", ex);
            }
        }
    }
}