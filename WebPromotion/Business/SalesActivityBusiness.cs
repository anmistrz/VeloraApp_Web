using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPromotion.Business.Interface;
using WebPromotion.Services.DTO;
using WebPromotion.Services.Interface;

namespace WebPromotion.Business
{
    public class SalesActivityBusiness : ISalesActivityBusiness
    {
        private readonly ISalesActivityService _salesActivityService;

        public SalesActivityBusiness(ISalesActivityService salesActivityService)
        {
            _salesActivityService = salesActivityService;
        }

        public async Task<IEnumerable<SalesActivityLogHandledRequestClientDTO>> getSalesActivityBySalesPerson(int salesPersonId, string activityType)
        {
            try
            {
                var response = await _salesActivityService.getSalesActivityBySalesPersonIdAsync(salesPersonId, activityType);
                return response;
            } catch (Exception ex)
            {
                // Handle exceptions
                throw new Exception("Error retrieving sales activities", ex);
            }
        }

        public async Task<SalesActivityClientDTO> getSalesActivityConsultationById(int id)
        {
            try
            {
                var response = await _salesActivityService.getSalesActivityConsultationByIdAsync(id);
                var convertDTO = new SalesActivityClientDTO
                {
                    ActivityLogId = response.ActivityLogId,
                    ConsultationId = response.ConsultationId,
                    TestDriveId = response.TestDriveId,
                    ActivityType = response.ActivityType,
                    ActivityDate = response.ActivityDate,
                    Details = response.Details
                };
                return convertDTO;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving sales activity consultation by ID: {ex.Message}", ex);
            }
        }

        public async Task<SalesActivityClientDTO> getSalesActivityTestDriveById(int id)
        {
            try
            {
                var result = await _salesActivityService.getSalesActivityTestDriveByIdAsync(id);
                var convertDTO = new SalesActivityClientDTO
                {
                    ActivityLogId = result.ActivityLogId,
                    ConsultationId = result.ConsultationId,
                    TestDriveId = result.TestDriveId,
                    ActivityType = result.ActivityType,
                    ActivityDate = result.ActivityDate,
                    Details = result.Details
                };
                return convertDTO;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving sales activity test drive by ID: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateResultConsultationAsync(int salesActivityId, string result)
        {
            try
            {
                var update = await _salesActivityService.UpdateResultConsultationAsync(salesActivityId, result);
                return update;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating result consultation: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateResultTestDriveAsync(int salesActivityId, string result)
        {
            try
            {
                var update = await _salesActivityService.UpdateResultTestDriveAsync(salesActivityId, result);
                return update;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating result test drive: {ex.Message}", ex);
            }
        }
    }
}