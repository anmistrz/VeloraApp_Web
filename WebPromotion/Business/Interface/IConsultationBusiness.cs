using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPromotion.Models;
using WebPromotion.Services.DTO;
using WebPromotion.ViewModels.ConsultHistoryView;

namespace WebPromotion.Business.Interface
{
    public interface IConsultationBusiness
    {
        public Task<ConsultHistory> CreateConsultHistoryGuest(ConsultHistoryInsertGuestViewModels consultation);
        public Task<IEnumerable<ConsultHistoryRequestClientDTO>> GetConsultHistoryRequestBySalesPerson(string salesPersonId);
        public Task<bool> DeleteConsultHistoryAfterHandled(int consultHistoryId, DeleteConsultRequestClientDTO model);
        public Task<bool> DeleteConsultHistoryBeforeHandled(int consultHistoryId, DeleteConsultRequestClientDTO model);
    }
}