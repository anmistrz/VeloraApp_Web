using System;
using DealerApi.Application.DTO;
using DealerApi.Entities.Models;

namespace DealerApi.Application.Interface;

public interface IConsultHistoryBL
{
    public Task<SuccessInsertUpdateDTO> CreateAsyncConsultHistoryGuest(Customer dataCustomer, ConsultHistory dataConsultHistory, DealerCar dataDealerCar);
    public Task<IEnumerable<ConsultHistoryRequestDTO>> GetConsultHistoryBySalesPersonIdAsync(int salesPersonId);
    public Task<bool> DeleteConsultHistoryAfterHandledAsync(int consultHistoryId, DeleteConsultRequestDTO deleteConsultRequest);
    public Task<bool> DeleteConsultHistoryBeforeHandledAsync(int consultHistoryId, DeleteConsultRequestDTO deleteConsultRequest);
    public Task<bool> UpdateConsultHistoryAsync(int consultHistoryId, ConsultHistory dataConsultHistory);
}
