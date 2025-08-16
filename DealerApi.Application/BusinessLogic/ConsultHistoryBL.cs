using System;
using DealerApi.Application.Interface;
using DealerApi.Application.DTO;
using DealerApi.Entities.Models;
using DealerApi.DAL.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace DealerApi.Application.BusinessLogic;

public class ConsultHistoryBL : IConsultHistoryBL
{
    private readonly IConsultHistory _consultHistoryDAL;

    public ConsultHistoryBL(IConsultHistory consultHistoryDAL)
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

    public async Task<bool> DeleteConsultHistoryAfterHandledAsync(int consultHistoryId, DeleteConsultRequestDTO deleteConsultRequest)
    {
        try
        {
            var body = new DeleteConsultRequestDTO
            {
                SalesPersonId = deleteConsultRequest.SalesPersonId,
                Reason = deleteConsultRequest.Reason,
                ConsultHistoryId = consultHistoryId,
                DealerId = deleteConsultRequest.DealerId
            };
            Console.WriteLine($"Deleting consult history with ID Business: {consultHistoryId}, SalesPersonId: {body.SalesPersonId}, DealerId: {body.DealerId}, Reason: {body.Reason}");
            var result = await _consultHistoryDAL.DeleteConsultHistoryAfterHandledAsync(consultHistoryId, body.Reason);
            return result;
        }
        catch (Exception ex)
        {
            // Handle exceptions as needed
            throw new InvalidOperationException("Error deleting consult history after handled", ex);
        }
    }

    public async Task<bool> DeleteConsultHistoryBeforeHandledAsync(int consultHistoryId, DeleteConsultRequestDTO deleteConsultRequest)
    {
        try
        {
            var body = new DeleteConsultRequestDTO
            {
                SalesPersonId = deleteConsultRequest.SalesPersonId,
                Reason = deleteConsultRequest.Reason,
                ConsultHistoryId = consultHistoryId,
                DealerId = deleteConsultRequest.DealerId
            };
            var result = await _consultHistoryDAL.DeleteConsultHistoryBeforeHandledAsync(consultHistoryId, body.SalesPersonId, body.DealerId, body.Reason);
            return result;
        }
        catch (Exception ex)
        {
            // Handle exceptions as needed
            throw new InvalidOperationException("Error deleting consult history before handled", ex);
        }
    }

    public async Task<IEnumerable<ConsultHistoryRequestDTO>> GetConsultHistoryBySalesPersonIdAsync(int salesPersonId)
    {
        try
        {
            var result = await _consultHistoryDAL.GetConsultHistoryBySalesPersonIdAsync(salesPersonId);
            if (result == null || !result.Any())
            {
                return Enumerable.Empty<ConsultHistoryRequestDTO>();
            }

            var convertDTO = result.Select(ch => new ConsultHistoryRequestDTO
            {
                ConsultHistoryId = ch.Item1.ConsultHistoryId,
                CustomerName = $"{ch.Item2.FirstName} {ch.Item2.LastName}",
                CarName = ch.Item3.CarModel,
                Note = ch.Item1.Note,
                Budget = ch.Item1.Budget ?? 0,
                ConsultDate = ch.Item1.ConsultDate,
                StatusConsultation = ch.Item1.StatusConsultation?.ToString() ?? "Pending"
            });

            return convertDTO;
        }
        catch (Exception ex)
        {
            // Handle exceptions as needed
            throw new InvalidOperationException("Error retrieving consult history", ex);
        }
    }

    public Task<bool> UpdateConsultHistoryAsync(int consultHistoryId, ConsultHistory dataConsultHistory)
    {
        throw new NotImplementedException();
    }
}
