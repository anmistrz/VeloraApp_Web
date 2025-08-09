using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.DAL.Interfaces;
using DealerApi.Application.DTO;
using DealerApi.Application.Interface;
using DealerApi.Entities.Models;

namespace DealerApi.Application.Services
{
    public class SalesPersonServices : ISalesPersonServices
    {
        private readonly ISalesPerson _salesPersonDAL;

        public SalesPersonServices(ISalesPerson salesPersonDAL)
        {
            _salesPersonDAL = salesPersonDAL;
        }

        public async Task<SalesPersonDTO> CreateSalesPersonAsync(SalesPersonDTO salesPerson)
        {
            try
            {
                var createSalesPerson = await _salesPersonDAL.CreateAsync(new SalesPerson
                {
                    DealerId = salesPerson.DealerId,
                    FullName = salesPerson.FullName,
                    Email = salesPerson.Email,
                    PhoneNumber = salesPerson.PhoneNumber,
                    UserName = salesPerson.UserName,
                    Password = salesPerson.Password, // Ensure to hash the password in a real application
                    Bonus = salesPerson.Bonus,
                    IsActive = salesPerson.IsActive
                });

                var result = new SalesPersonDTO
                {
                    SalesPersonId = createSalesPerson.SalesPersonId,
                    DealerId = createSalesPerson.DealerId,
                    FullName = createSalesPerson.FullName,
                    Email = createSalesPerson.Email,
                    PhoneNumber = createSalesPerson.PhoneNumber,
                    UserName = createSalesPerson.UserName,
                    Password = createSalesPerson.Password, // Ensure to hash the password in a real application
                    Bonus = createSalesPerson.Bonus,
                    IsActive = createSalesPerson.IsActive
                };

                return result;
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error creating sales person", ex);
            }
        }

        public async Task<bool> DeleteSalesPersonAsync(int id)
        {
            try
            {
                return await _salesPersonDAL.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error deleting sales person", ex);
            }

        }

        public async Task<IEnumerable<SalesPersonDTO>> GetAllSalesPersonsAsync()
        {
            try
            {
                var getAllSalesPerson = await _salesPersonDAL.GetAllAsync();

                if (getAllSalesPerson == null || !getAllSalesPerson.Any())
                {
                    return Enumerable.Empty<SalesPersonDTO>();
                }

                return getAllSalesPerson.Select(s => new SalesPersonDTO
                {
                    SalesPersonId = s.SalesPersonId,
                    DealerId = s.DealerId,
                    FullName = s.FullName,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    UserName = s.UserName,
                    Password = s.Password,
                    Bonus = s.Bonus,
                    IsActive = s.IsActive
                });
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error retrieving all sales persons", ex);
            }
        }

        public async Task<SalesPersonDTO> GetSalesPersonByIdAsync(int id)
        {
            try
            {
                var salesPerson = await _salesPersonDAL.GetByIdAsync(id);
                if (salesPerson == null)
                {
                    return null;
                }

                var result = new SalesPersonDTO
                {
                    SalesPersonId = salesPerson.SalesPersonId,
                    DealerId = salesPerson.DealerId,
                    FullName = salesPerson.FullName,
                    Email = salesPerson.Email,
                    PhoneNumber = salesPerson.PhoneNumber,
                    UserName = salesPerson.UserName,
                    Password = salesPerson.Password,
                    Bonus = salesPerson.Bonus,
                    IsActive = salesPerson.IsActive
                };

                return result;
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error retrieving sales person by ID", ex);
            }
        }

        public Task<IEnumerable<SalesPersonDTO>> GetSalesPersonsByDealerIdAsync(int dealerId)
        {
            try
            {
                var salesPersons = _salesPersonDAL.GetSalesPersonsByDealerIdAsync(dealerId).Result;

                if (salesPersons == null || !salesPersons.Any())
                {
                    return Task.FromResult<IEnumerable<SalesPersonDTO>>(Enumerable.Empty<SalesPersonDTO>());
                }

                var result = salesPersons.Select(s => new SalesPersonDTO
                {
                    SalesPersonId = s.SalesPersonId,
                    DealerId = s.DealerId,
                    FullName = s.FullName,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    UserName = s.UserName,
                    Password = s.Password,
                    Bonus = s.Bonus,
                    IsActive = s.IsActive
                });

                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error retrieving sales persons by dealer ID", ex);
            }
        }

        public async Task<SalesPersonDTO> UpdateSalesPersonAsync(SalesPersonDTO salesPerson)
        {
            try
            {
                var updatedSalesPerson = await _salesPersonDAL.UpdateAsync(new SalesPerson
                {
                    SalesPersonId = salesPerson.SalesPersonId,
                    DealerId = salesPerson.DealerId,
                    FullName = salesPerson.FullName,
                    Email = salesPerson.Email,
                    PhoneNumber = salesPerson.PhoneNumber,
                    UserName = salesPerson.UserName,
                    Password = salesPerson.Password,
                    Bonus = salesPerson.Bonus,
                    IsActive = salesPerson.IsActive
                });

                if (updatedSalesPerson == null)
                {
                    return null;
                }

                return new SalesPersonDTO
                {
                    SalesPersonId = updatedSalesPerson.SalesPersonId,
                    DealerId = updatedSalesPerson.DealerId,
                    FullName = updatedSalesPerson.FullName,
                    Email = updatedSalesPerson.Email,
                    PhoneNumber = updatedSalesPerson.PhoneNumber,
                    UserName = updatedSalesPerson.UserName,
                    Password = updatedSalesPerson.Password,
                    Bonus = updatedSalesPerson.Bonus,
                    IsActive = updatedSalesPerson.IsActive
                };
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error updating sales person", ex);
            }
        }
    
        public Task<IEnumerable<SalesPersonDTO>> GetSalesPersonByEmailAsync(string email)
        {
            try
            {
                var salesPersons = _salesPersonDAL.GetSalesPersonByEmailAsync(email).Result;

                if (salesPersons == null || !salesPersons.Any())
                {
                    return Task.FromResult<IEnumerable<SalesPersonDTO>>(Enumerable.Empty<SalesPersonDTO>());
                }

                var result = salesPersons.Select(s => new SalesPersonDTO
                {
                    SalesPersonId = s.SalesPersonId,
                    DealerId = s.DealerId,
                    FullName = s.FullName,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    UserName = s.UserName,
                    Password = s.Password,
                    Bonus = s.Bonus,
                    IsActive = s.IsActive
                });

                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error retrieving sales person by email", ex);
            }
        }
    }
}