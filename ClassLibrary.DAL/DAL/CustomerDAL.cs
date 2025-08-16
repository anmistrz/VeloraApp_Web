using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.DAL.Context;
using DealerApi.DAL.Interfaces;
using DealerApi.Entities.Models;

namespace ClassLibrary.DAL
{
    public class CustomerDAL : ICustomer
    {
        private readonly DealerRndDBContext _context;

        public CustomerDAL(DealerRndDBContext context)
        {
            _context = context;
        }

        public async Task<Customer> CreateAsync(Customer entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Customer>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Customers.FindAsync(id);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new Exception("Error retrieving customer by ID", ex);
            }
        }

        public Task<IEnumerable<Customer>> GetBySearchAsync(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> UpdateAsync(Customer entity)
        {
            throw new NotImplementedException();
        }
    }
}