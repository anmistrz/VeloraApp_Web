using ClassLibrary.DAL.Interfaces;
using DealerApi.DAL.Context;
using DealerApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassLibrary.DAL
{
    public class DealerDAL : IDealer
    {
        private readonly DealerRndDBContext _context;
        public DealerDAL(DealerRndDBContext context)
        {
            _context = context;
        }


        public Task<Dealer> CreateAsync(Dealer entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Dealer>> GetAllAsync()
        {
            try
            {
                return await _context.Dealers.ToListAsync();
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new Exception("Error retrieving dealers", ex);
            }
        }

        public Task<Dealer> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Dealer>> GetBySearchAsync(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<(Dealer,float average)>> GetMostPopularDealersAsync(int top)
        {
            try
            {
                var result = await _context.Dealers
                    .Join(_context.CustomerRatings,
                        dealer => dealer.DealerId,
                        rating => rating.DealerId,
                        (dealer, rating) => new { dealer, rating })
                    .GroupBy(x => new
                    {
                        x.dealer.DealerId,
                        x.dealer.DealerName,
                        x.dealer.Location,
                        x.dealer.Province,
                        x.dealer.City,
                        x.dealer.Address,
                        x.dealer.PhoneNumber,
                    })
                    .Select(g => new
                    {
                        DealerId = g.Key.DealerId,
                        DealerName = g.Key.DealerName,
                        Location = g.Key.Location,
                        Province = g.Key.Province,
                        City = g.Key.City,
                        Address = g.Key.Address,
                        PhoneNumber = g.Key.PhoneNumber,
                        AverageRating = g.Average(x => x.rating.RatingValue),
                    })
                    .OrderByDescending(x => x.AverageRating)
                    .Take(top)
                    .AsNoTracking()
                    .ToListAsync();

                return result.Select(x =>
                (
                    new Dealer
                    {
                        DealerId = x.DealerId,
                        DealerName = x.DealerName,
                        Location = x.Location,
                        Province = x.Province,
                        City = x.City,
                        Address = x.Address,
                        PhoneNumber = x.PhoneNumber
                    },
                    (float)x.AverageRating
                ));
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new InvalidOperationException("Error retrieving most popular dealers", ex);
            }
        }

        public Task<Dealer> UpdateAsync(Dealer entity)
        {
            throw new NotImplementedException();
        }
    }
}