using DealerApi.Application.DTO;
using WebPromotion.Models;
using WebPromotion.Services.DTO;
using WebPromotion.ViewModels.ConsultHistoryView;

namespace WebPromotion.Services.Consultation
{
    public interface IConsultationServices
    {
        public Task<ConsultHistory> CreateAsyncConsultHistoryGuest(ConsultationInsertGuestDTO model);
        public Task<IEnumerable<ConsultHistoryRequestClientDTO>> GetConsultHistoryRequestBySalesPersonAsync(string salesPersonId);
        public Task<bool> DeleteConsultHistoryAfterHandledAsync(int consultHistoryId, DeleteConsultRequestDTO model);
        public Task<bool> DeleteConsultHistoryBeforeHandledAsync(int consultHistoryId, DeleteConsultRequestDTO model);
    }
}
