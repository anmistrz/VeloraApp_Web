using System;
using DealerApi.Application.Interface;
using DealerApi.Application.DTO;
using DealerApi.Entities.Models;
using DealerApi.DAL.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace DealerApi.Application.Services;

public class ConsultHistoryServices : IConsultHistoryServices
{
    private readonly IConsultHistory _consultHistoryDAL;

    public ConsultHistoryServices(IConsultHistory consultHistoryDAL)
    {
        _consultHistoryDAL = consultHistoryDAL ?? throw new ArgumentNullException(nameof(consultHistoryDAL));
    }


    public Task<SuccessInsertUpdateDTO> CreateAsyncConsultHistoryGuest(Customer dataCustomer, ConsultHistory dataConsultHistory, DealerCar dataDealerCar)
    {
        try
        {

            if (dataCustomer == null)
            {
                throw new ArgumentNullException(nameof(dataCustomer), "Customer cannot be null");
            }

            // Validate required fields
            if (string.IsNullOrWhiteSpace(dataCustomer.FirstName))
                throw new ArgumentException("FirstName is required");
            if (string.IsNullOrWhiteSpace(dataCustomer.LastName))
                throw new ArgumentException("LastName is required");
            if (string.IsNullOrWhiteSpace(dataCustomer.Email))
                throw new ArgumentException("Email is required");
            if (string.IsNullOrWhiteSpace(dataCustomer.PhoneNumber))
                throw new ArgumentException("PhoneNumber is required");
            if (dataDealerCar.DealerId <= 0)
                throw new ArgumentException("DealerId must be greater than 0");

            // Map from Application DTO to DAL DTO
            var dtCustomer = new Customer
            {
                FirstName = dataCustomer.FirstName,
                LastName = dataCustomer.LastName,
                Email = dataCustomer.Email,
                PhoneNumber = dataCustomer.PhoneNumber,
                IsGuest = true // Assuming this is a guest
            };

            var dtConsultHistory = new ConsultHistory
            {
                DealerCarUnitId = dataConsultHistory.DealerCarUnitId,
                ConsultDate = dataConsultHistory.ConsultDate,
                Note = dataConsultHistory.Note,
                SalesPersonId = dataConsultHistory.SalesPersonId,
                Budget = dataConsultHistory.Budget
            };

            var dtDealerCar = new DealerCar
            {
                DealerId = dataDealerCar.DealerId,
                CarId = dataDealerCar.CarId // Assuming DealerCarUnitId maps to CarId
            };

            var result = _consultHistoryDAL.CreateConsultHistoryGuestAsync(dtCustomer, dtConsultHistory, dtDealerCar);

            Console.WriteLine($"ConsultHistoryServices: Result: {JsonSerializer.Serialize(result)}");
            
            if (result == null)
            {
                throw new InvalidOperationException("Failed to create consult history for guest");
            }
            else
            {
                return Task.FromResult(new SuccessInsertUpdateDTO
                {
                    Id = result.Id,
                    Message = "Consult history created successfully"
                });
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions as needed
            throw new InvalidOperationException("Error creating consult history for guest", ex);
        }
    }
}
