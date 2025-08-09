using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.DAL.Interfaces;
using DealerApi.DAL.Context;
using DealerApi.Entities.Models;

namespace ClassLibrary.DAL.DAL
{
    public class SalesPersonDAL : ISalesPerson
    {
        private readonly DealerRndDBContext _context;

        public SalesPersonDAL(DealerRndDBContext context)
        {
            _context = context;
        }

        public Task<SalesPerson> CreateAsync(SalesPerson entity)
        {
            try
            {
                _context.SalesPeople.Add(entity);
                _context.SaveChanges();
                return Task.FromResult(entity);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("Error creating SalesPerson", ex);
            }
        }

        public Task<bool> DeleteAsync(int id)
        {
            try
            {
                var salesPerson = _context.SalesPeople.Find(id);
                if (salesPerson == null) return Task.FromResult(false);
                _context.SalesPeople.Remove(salesPerson);
                _context.SaveChanges();
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("Error deleting SalesPerson", ex);
            }
        }

        public Task<IEnumerable<SalesPerson>> GetAllAsync()
        {
            try
            {
                var salesPeople = _context.SalesPeople.ToList();
                return Task.FromResult(salesPeople.AsEnumerable());
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("Error retrieving all SalesPeople", ex);
            }
        }

        public Task<SalesPerson> GetByIdAsync(int id)
        {
            try
            {
                var salesPerson = _context.SalesPeople.Find(id);
                return Task.FromResult(salesPerson);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("Error retrieving SalesPerson", ex);
            }
        }

        public Task<IEnumerable<SalesPerson>> GetSalesPersonByEmailAsync(string email)
        {
            try
            {
                var salesPerson = _context.SalesPeople
                    .Where(sp => sp.Email == email)
                    .ToList();
                return Task.FromResult(salesPerson.AsEnumerable());
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("Error retrieving SalesPerson by email", ex);
            }
        }

        public Task<IEnumerable<SalesPerson>> GetSalesPersonsByDealerIdAsync(int dealerId)
        {
            try
            {
                var salesPeople = _context.SalesPeople
                    .Where(sp => sp.DealerId == dealerId)
                    .ToList();
                return Task.FromResult(salesPeople.AsEnumerable());
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("Error retrieving SalesPeople by DealerId", ex);
            }
        }

        public Task<SalesPerson> UpdateAsync(SalesPerson entity)
        {
            try
            {
                _context.SalesPeople.Update(entity);
                _context.SaveChanges();
                return Task.FromResult(entity);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("Error updating SalesPerson", ex);
            }
        }
    }
}