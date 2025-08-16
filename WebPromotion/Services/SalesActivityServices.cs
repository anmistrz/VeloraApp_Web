using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPromotion.Models;
using WebPromotion.Services.DTO;
using WebPromotion.Services.Interface;

namespace WebPromotion.Services
{
    public class SalesActivityServices : ISalesActivityService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public SalesActivityServices(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["ApiBaseUrl"]);
        }

        public async Task<List<SalesActivityLogHandledRequestClientDTO>> getSalesActivityBySalesPersonIdAsync(int salesPersonId, string activityType)
        {
            try
            {
                var salesActivities = await _httpClient.GetFromJsonAsync<List<SalesActivityLogHandledRequestClientDTO>>($"SalesActivity/all-sales-activities/{salesPersonId}/{activityType}");
                return salesActivities;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                throw new Exception("Error retrieving sales activities", ex);
            }
        }

        public async Task<SalesActivityLog> getSalesActivityConsultationByIdAsync(int consultationId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<SalesActivityLog>($"SalesActivity/sales-activities-consultation/{consultationId}");
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving sales activity consultation by ID: {ex.Message}", ex);
            }
        }

        public async Task<SalesActivityLog> getSalesActivityTestDriveByIdAsync(int testDriveId)
        {
            var response = await _httpClient.GetFromJsonAsync<SalesActivityLog>($"SalesActivity/sales-activities-test-drive/{testDriveId}");
            return response;
        }

        public async Task<bool> UpdateResultConsultationAsync(int salesActivityId, string result)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"SalesActivity/update-consultation-result/{salesActivityId}", result);
                return response.IsSuccessStatusCode;
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
                var response = await _httpClient.PostAsJsonAsync($"SalesActivity/update-test-drive-result/{salesActivityId}", result);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Test drive result updated successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to update test drive result: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating result test drive: {ex.Message}", ex);
            }
        }
    }
}