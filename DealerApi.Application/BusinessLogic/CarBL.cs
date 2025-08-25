using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.DAL.Interfaces;
using DealerApi.Application.DTO;
using DealerApi.Application.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;

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
                throw new InvalidOperationException("Error retrieving car options", ex);
            }
        }

        public Task<IEnumerable<SelectListItem>> GetCarSelectListItems()
        {
            try
            {
                var cars = _carDAL.GetAllAsync().Result; // Blocking call, consider using await instead
                return Task.FromResult(cars.Select(car => new SelectListItem
                {
                    Value = car.CarId.ToString(),
                    Text = car.CarModel
                }));
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new InvalidOperationException("Error retrieving car select list items", ex);
            }
        }

        public async Task<IEnumerable<ListMostCarTestDriveDTO>> ListMostCarTestDrivesTop(int top)
        {
            try
            {
                var result = await _carDAL.GetMostTestDriveCarsWithDealersAsync(top);
                var convertDTO = result.Select(x => new ListMostCarTestDriveDTO
                {
                    carId = x.Item1.CarId,
                    make = x.Item1.Make,
                    carModel = x.Item1.CarModel,
                    carType = x.Item1.CarType,
                    year = x.Item1.Year,
                    engineSize = x.Item1.EngineSize,
                    fuelType = x.Item1.FuelType,
                    transmission = x.Item1.Transmission,
                    color = x.Item1.Color,
                    description = x.Item1.Description,
                    dealerDetail = new DealerCustomDTO
                    {
                        dealerId = x.Item2.DealerId,
                        dealerName = x.Item2.DealerName,
                        dealerLocation = x.Item2.Location,
                        dealerProvince = x.Item2.Province,
                        dealerCity = x.Item2.City,
                        dealerAddress = x.Item2.Address,
                        dealerContact = x.Item2.PhoneNumber,
                        dealerEmail = x.Item2.Email
                    },
                    dealerCarDetail = new DealerCarCustomDTO
                    {
                        dealerCarId = x.Item3.DealerCarId,
                        stock = x.Item3.Stock,
                        dealerPrice = (double)x.Item3.DealerPrice,
                        tax = (double)(x.Item3.Tax ?? 0),
                        status = x.Item3.Status
                    },
                    TestDriveCount = x.Item4
                });

                return convertDTO;
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new InvalidOperationException("Error retrieving most test drive cars with dealers", ex);
            }
        }
    }
}