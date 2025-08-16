using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ClassLibrary.DAL.Interfaces;
using DealerApi.Application.DTO;
using DealerApi.Application.Interface;
using DealerApi.DAL.Context;
using DealerApi.Entities.Models;

namespace DealerApi.Application.BusinessLogic
{
    public class SalesActivityBL : ISalesActivityLogBL
    {
        private readonly ISalesActivityLog _salesActivityDAL;

        public SalesActivityBL(ISalesActivityLog salesActivityDAL)
        {
            _salesActivityDAL = salesActivityDAL;
        }

        public async Task<IEnumerable<SalesActivityLogHandledRequestDTO>> GetAllSalesActivitiesBySalesPersonAsync(int salesPersonId, string NotificationType)
        {
            try
            {
                var result = await _salesActivityDAL.GetAllSalesActivitiesBySalesPersonAsync(salesPersonId, NotificationType);
                if (result == null || !result.Any())
                {
                    return Enumerable.Empty<SalesActivityLogHandledRequestDTO>();
                }

                var convertDTO = result.Select(x => new SalesActivityLogHandledRequestDTO
                {
                    SalesActivityLogId = x.Item1.ActivityLogId,
                    ConsultationId = x.Item1.ConsultationId,
                    TestDriveId = x.Item1.TestDriveId,
                    CustomerName = $"{x.Item2.FirstName} {x.Item2.LastName}",
                    CarName = x.Item3.CarModel,
                    ActivityType = x.Item1.ActivityType,
                    ActivityDate = x.Item1.ActivityDate,
                    Details = x.Item1.Details
                });

                return convertDTO;
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                throw new Exception("Error fetching sales activities", ex);
            }
        }
        public async Task<SalesActivityDTO> GetSalesActivityConsultationByIdAsync(int consultationId)
        {
            try
            {
                var result = await _salesActivityDAL.GetSalesActivityConsultationById(consultationId);
                Console.WriteLine($"resultttServicesss: {JsonSerializer.Serialize(result)}");

                var convertData = new SalesActivityDTO
                {
                    ActivityLogId = result.ActivityLogId,
                    ConsultationId = result.ConsultationId,
                    ActivityType = result.ActivityType,
                    ActivityDate = result.ActivityDate,
                    Details = result.Details
                };
                return convertData;
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                throw new Exception("Error fetching sales activity consultation by ID", ex);
            }
        }

        public async Task<SalesActivityDTO> GetSalesActivityTestDriveByIdAsync(int testDriveId)
        {
            var result = await _salesActivityDAL.GetSalesActivityTestDriveById(testDriveId);
            return new SalesActivityDTO
            {
                ActivityLogId = result.ActivityLogId,
                TestDriveId = result.TestDriveId,
                ActivityType = result.ActivityType,
                ActivityDate = result.ActivityDate,
                Details = result.Details
            };
        }

        public async Task<bool> UpdateResultConsultationAsync(int salesActivityId, string result)
        {
            try
            {
                return await _salesActivityDAL.UpdateResultConsultationAsync(salesActivityId, result);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                throw new Exception("Error updating sales activity", ex);
            }
        }

        public async Task<bool> UpdateResultTestDriveAsync(int salesActivityId, string result)
        {
            try
            {
                return await _salesActivityDAL.UpdateResultTestDriveAsync(salesActivityId, result);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                throw new Exception("Error updating sales activity", ex);
            }
        }
    }
}