using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.DAL.Interfaces;
using DealerApi.DAL.Context;
using DealerApi.DAL.Interfaces;
using DealerApi.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DealerApi.DAL.DAL
{
    public class ConsultHistoryDAL : IConsultHistory
    {
        private readonly DealerRndDBContext _context;

        public ConsultHistoryDAL(DealerRndDBContext context)
        {
            _context = context;
        }


        public async Task<ConsultHistory> CreateAsync(ConsultHistory entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ConsultHistory> CreateConsultHistoryGuestAsync(Customer dataCustomer, ConsultHistory dataConsultHistory, DealerCar dataDealerCar)
        {
            try
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    Console.WriteLine($"ConsultHistoryDAL: Processing request for {dataCustomer.Email}");
                    var validateCustomer = _context.Customers
                        .FirstOrDefault(c => c.Email == dataCustomer.Email && c.IsGuest);
                        
                    Console.WriteLine($"ConsultHistoryDAL: Validating customer: {dataCustomer.Email}");

                    if (validateCustomer == null)
                    {
                        var customer = new CustomerTemp
                        {
                            FirstName = dataCustomer.FirstName,
                            LastName = dataCustomer.LastName,
                            Email = dataCustomer.Email,
                            PhoneNumber = dataCustomer.PhoneNumber,
                            UserName = "Guest",
                        };

                        Console.WriteLine($"Customer created: {customer.FirstName} {customer.LastName} - {customer.Email}");

                        _context.CustomerTemps.Add(customer);
                        await _context.SaveChangesAsync();

                        var findCustomer = await _context.CustomerTemps
                            .FirstOrDefaultAsync(c => c.Email == dataCustomer.Email);

                        if (findCustomer == null)
                        {
                            throw new Exception("Failed to create customer.");
                        }
                        dataCustomer.CustomerId = findCustomer.CustomerTempId;
                    }
                    else
                    {
                        // Use existing customer
                        dataCustomer.CustomerId = validateCustomer.CustomerId;
                        Console.WriteLine($"Using existing customer: {validateCustomer.FirstName} {validateCustomer.LastName} - {validateCustomer.Email}");
                    }

                    var consultHistory = new ConsultHistory
                    {
                        CustomerId = dataCustomer.CustomerId,
                        DealerCarUnitId = dataConsultHistory.DealerCarUnitId,
                        SalesPersonId = dataConsultHistory.SalesPersonId == 0 ? null : dataConsultHistory.SalesPersonId,
                        Budget = dataConsultHistory.Budget,
                        ConsultDate = dataConsultHistory.ConsultDate,
                        Note = dataConsultHistory.Note
                    };

                    _context.ConsultHistories.Add(consultHistory);
                    await _context.SaveChangesAsync();

                    var notification = new Notification
                    {
                        CustomerId = dataCustomer.CustomerId,
                        DealerId = dataDealerCar.DealerId,
                        ConsultHistoryId = consultHistory.ConsultHistoryId,
                        NotificationType = "Request Consultation",
                        Message = $"New consultation request from {dataCustomer.FirstName} {dataCustomer.LastName} on {consultHistory.ConsultDate.ToShortDateString()} at dealer {dataDealerCar.DealerId}. Contact: {dataCustomer.Email}, {dataCustomer.PhoneNumber}",
                        IsRead = false,
                        Priority = 1,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.Notifications.Add(notification);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    var result = new ConsultHistory
                    {
                        CustomerId = consultHistory.CustomerId,
                        DealerCarUnitId = consultHistory.DealerCarUnitId,
                        SalesPersonId = consultHistory.SalesPersonId,
                        Budget = consultHistory.Budget,
                        ConsultDate = consultHistory.ConsultDate,
                        Note = consultHistory.Note,
                        ConsultHistoryId = consultHistory.ConsultHistoryId
                    };
                    return result;
                }
           } 
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log them)
                throw new Exception("Error creating consultation history guest", ex);
            }
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ConsultHistory>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ConsultHistory> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ConsultHistory> UpdateAsync(ConsultHistory entity)
        {
            throw new NotImplementedException();
        }
    }
}