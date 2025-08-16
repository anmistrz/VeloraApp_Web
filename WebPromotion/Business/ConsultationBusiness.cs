using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.Application.DTO;
using WebPromotion.Business.Interface;
using WebPromotion.Models;
using WebPromotion.Services.Consultation;
using WebPromotion.Services.DTO;

namespace WebPromotion.Business
{
    public class ConsultationBusiness : IConsultationBusiness
    {
        public readonly IConsultationServices _consultationServices;

        public ConsultationBusiness(IConsultationServices consultationServices)
        {
            _consultationServices = consultationServices;
        }

        public Task<ConsultHistory> CreateConsultHistoryGuest(ConsultationInsertGuestDTO consultation)
        {
            try 
            {
                return _consultationServices.CreateAsyncConsultHistoryGuest(consultation);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed, e.g., logging
                throw new Exception("An error occurred while creating consultation history.", ex);
            }
        }

        public Task<bool> DeleteConsultHistoryAfterHandled(int consultHistoryId, DeleteConsultRequestClientDTO model)
        {
            try
            {
                Console.WriteLine($"Deleting consult history with ID: {consultHistoryId}");
                if (model == null)
                {
                    throw new ArgumentException("Model must not be null", nameof(model));
                }
                var mappedModel = new DeleteConsultRequestDTO
                {
                    ConsultHistoryId = model.ConsultHistoryId,
                    Reason = model.Reason,
                    SalesPersonId = model.SalesPersonId,
                    DealerId = model.DealerId
                };
                return _consultationServices.DeleteConsultHistoryAfterHandledAsync(consultHistoryId, mappedModel);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed, e.g., logging
                throw new Exception("An error occurred while deleting consultation history after handled.", ex);
            }
        }

        public Task<bool> DeleteConsultHistoryBeforeHandled(int consultHistoryId, DeleteConsultRequestClientDTO model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentException("Model must not be null", nameof(model));
                }
                var mappedModel = new DeleteConsultRequestDTO
                {
                    ConsultHistoryId = model.ConsultHistoryId,
                    Reason = model.Reason,
                    SalesPersonId = model.SalesPersonId,
                    DealerId = model.DealerId
                };
                return _consultationServices.DeleteConsultHistoryBeforeHandledAsync(consultHistoryId, mappedModel);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed, e.g., logging
                throw new Exception("An error occurred while deleting consultation history before handled.", ex);
            }
        }

        public Task<IEnumerable<ConsultHistoryRequestClientDTO>> GetConsultHistoryRequestBySalesPerson(string salesPersonId)
        {
            try
            {
                return _consultationServices.GetConsultHistoryRequestBySalesPersonAsync(salesPersonId);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed, e.g., logging
                throw new Exception("An error occurred while fetching consultation history requests.", ex);
            }
        }
    }
}