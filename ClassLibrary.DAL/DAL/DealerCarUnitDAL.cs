using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.DAL.Interfaces;
using DealerApi.DAL.Context;
using DealerApi.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.DAL.DAL
{
    public class DealerCarUnitDAL : IDealerCarUnit
    {
        private readonly DealerRndDBContext _context;

        public DealerCarUnitDAL(DealerRndDBContext context)
        {
            _context = context;
        }
        public async Task<DealerCarUnit> CreateAsync(DealerCarUnit entity)
        {
            try
            {
                _context.DealerCarUnits.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                throw new Exception("Error creating DealerCarUnit", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.DealerCarUnits.FindAsync(id);
                if (entity == null) return false;

                _context.DealerCarUnits.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                throw new Exception("Error deleting DealerCarUnit", ex);
            }
        }

        public async Task<int> DeleteDealerCarUnitWithConsultHistoryAsync(int dealerCarUnitId)
        {
            try
            {
                var consults = _context.ConsultHistories
                    .Where(c => c.DealerCarUnitId == dealerCarUnitId);
                _context.ConsultHistories.RemoveRange(consults);

                // Hapus DealerCarUnit
                var unit = await _context.DealerCarUnits.FindAsync(dealerCarUnitId);
                if (unit != null)
                {
                    _context.DealerCarUnits.Remove(unit);
                }

                await _context.SaveChangesAsync();
                return dealerCarUnitId;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                throw new Exception("Error deleting DealerCarUnit with ConsultHistory", ex);
            }
        }

        public Task<IEnumerable<DealerCarUnit>> GetAllAsync()
        {
            try
            {
                return Task.FromResult(_context.DealerCarUnits.AsEnumerable());
            }
            catch (Exception ex)
            {
                // Handle exceptions
                throw new Exception("Error fetching all DealerCarUnits", ex);
            }
        }

        public async Task<DealerCarUnit> GetByIdAsync(int id)
        {
            try
            {
                return await _context.DealerCarUnits.FindAsync(id);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                throw new Exception("Error fetching DealerCarUnit by ID", ex);
            }
        }

        public async Task<DealerCarUnit> UpdateAsync(DealerCarUnit entity)
        {
            try
            {
                _context.DealerCarUnits.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                throw new Exception("Error updating DealerCarUnit", ex);
            }
        }
    }
}