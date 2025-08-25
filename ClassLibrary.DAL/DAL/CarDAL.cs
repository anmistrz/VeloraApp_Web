using System;
using ClassLibrary.DAL.Interfaces;
using DealerApi.DAL.Context;
using DealerApi.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DealerApi.DAL;

public class CarDAL : ICar
{
    private readonly DealerRndDBContext _context;
    public CarDAL(DealerRndDBContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task<Car> CreateAsync(Car entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Car>> GetAllAsync()
    {
        try
        {
            return await _context.Cars.ToListAsync();
        }
        catch (Exception ex)
        {
            // Handle exceptions as needed
            throw new Exception("Error retrieving cars", ex);
        }
    }

    public async Task<Car> GetByIdAsync(int id)
    {
        try
        {
            return await _context.Cars.FindAsync(id);
        }
        catch (Exception ex)
        {
            // Handle exceptions as needed
            throw new Exception("Error retrieving car by ID", ex);
        }
    }

    public async Task<List<(Car, Dealer, DealerCar, int totalCount)>> GetMostTestDriveCarsWithDealersAsync(int top)
    {
        try
        {
            var result = await _context.Cars
                .Join(_context.DealerCars,
                    car => car.CarId,
                    dealerCar => dealerCar.CarId,
                    (car, dealerCar) => new { car, dealerCar })
                .Join(_context.DealerCarUnits,
                    cd => cd.dealerCar.DealerCarId,
                    dealerCarUnit => dealerCarUnit.DealerCarId,
                    (cd, dealerCarUnit) => new
                    {
                        Car = cd.car,
                        DealerCar = cd.dealerCar,
                        DealerCarUnit = dealerCarUnit
                    })
                .Join(_context.TestDrives,
                    cd => cd.DealerCarUnit.DealerCarUnitId,
                    testDrive => testDrive.DealerCarUnitId,
                    (cd, testDrive) => new
                    {
                        Car = cd.Car,
                        DealerCar = cd.DealerCar,
                        DealerCarUnit = cd.DealerCarUnit,
                        TestDrive = testDrive
                    })
                .Join(_context.Dealers,
                    cd => cd.DealerCar.DealerId,
                    dealer => dealer.DealerId,
                    (cd, dealer) => new
                    {
                        Car = cd.Car,
                        DealerCar = cd.DealerCar,
                        DealerCarUnit = cd.DealerCarUnit,
                        TestDrive = cd.TestDrive,
                        Dealer = dealer
                    })
                .Where(x => x.TestDrive.Status == "Completed")
                .GroupBy(x => new { x.Car.CarId, x.Dealer.DealerId })
                .Select(g => new
                {
                    Car = g.Select(x => x.Car).FirstOrDefault(),
                    Dealer = g.Select(x => x.Dealer).FirstOrDefault(),
                    DealerCar = g.Select(x => x.DealerCar).FirstOrDefault(),
                    TestDrive = g.Count()
                })
                .OrderByDescending(x => x.TestDrive)
                .Take(top)
                .ToListAsync();

            if (result == null || result.Count == 0)
            {
                return new List<(Car, Dealer, DealerCar, int)>();
            }

            return result.Select(x => (x.Car, x.Dealer, x.DealerCar, x.TestDrive)).ToList();
        }
        catch (Exception ex)
        {
            // Handle exceptions as needed
            throw new Exception("Error retrieving most test-driven cars with dealers", ex);
        }
    }

    public Task<Car> UpdateAsync(Car entity)
    {
        throw new NotImplementedException();
    }
}
