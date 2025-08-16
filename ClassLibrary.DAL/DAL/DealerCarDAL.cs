using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.DAL.Interfaces;
using DealerApi.DAL.Context;
using DealerApi.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.DAL
{
    public class DealerCarDAL : IDealerCar
    {
        private readonly DealerRndDBContext _context;
        public DealerCarDAL(DealerRndDBContext context)
        {
            _context = context;
        }


        public Task<DealerCarUnit> CreateAsync(DealerCarUnit entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DealerCarUnit>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<DealerCarUnit> GetByIdAsync(int id)
        {
            try
            {
                return await _context.DealerCarUnits.FindAsync(id);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new Exception("Error retrieving dealer car unit by ID", ex);
            }
        }

        public async Task<IEnumerable<(Dealer, Car, DealerCarUnit)>> GetOptionsDealerCarUnitByStatusAsync(string status)
        {
            try
            {
                var query = from dealerCar in _context.DealerCars
                            join dealerCarUnit in _context.DealerCarUnits on dealerCar.DealerCarId equals dealerCarUnit.DealerCarId
                            join dealer in _context.Dealers on dealerCar.DealerId equals dealer.DealerId
                            join car in _context.Cars on dealerCar.CarId equals car.CarId
                            where  dealerCarUnit.Status == status
                            select new { Dealer = dealer, Car = car, DealerCarUnit = dealerCarUnit };

                var result = await query.ToListAsync();
                return result.Select(x => (x.Dealer, x.Car, x.DealerCarUnit));
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new Exception("Error retrieving dealer car options", ex);
            }
        }

        public Task<DealerCarUnit> UpdateAsync(DealerCarUnit entity)
        {
            throw new NotImplementedException();
        }
    }
}       