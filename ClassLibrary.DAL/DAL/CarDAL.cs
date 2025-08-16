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

    public Task<Car> UpdateAsync(Car entity)
    {
        throw new NotImplementedException();
    }
}
