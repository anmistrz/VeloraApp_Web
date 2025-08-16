using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DealerApi.Application.DTO;
using Microsoft.EntityFrameworkCore;
using WebPromotion.Models;
using WebPromotion.Services.DTO;
using WebPromotion.ViewModels.ConsultHistoryView;

namespace WebPromotion.Services.Consultation
{
    public class ConsultationServices : IConsultationServices
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public ConsultationServices(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["ApiBaseUrl"]);
        }


        public Task<ConsultHistory> CreateAsyncConsultHistoryGuest(ConsultationInsertGuestDTO model)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(model);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                Console.WriteLine($"Request Data: {jsonContent}");

                var response = _httpClient.PostAsync("ConsultHistory/create-guest", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadFromJsonAsync<ConsultHistory>();
                }
                else
                {
                    throw new Exception($"Error creating consult history: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while creating consult history: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteConsultHistoryAfterHandledAsync(int consultHistoryId, DeleteConsultRequestDTO model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentException("Model must not be null", nameof(model));
                }
                var jsonContent = JsonSerializer.Serialize(model);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                Console.WriteLine($"Deleting consult history with ID Servicess: {consultHistoryId}, SalesPersonId: {model.SalesPersonId}, DealerId: {model.DealerId}, Reason: {model.Reason}");

                var response = await _httpClient.PostAsync($"ConsultHistory/delete-after-handled/{consultHistoryId}", content);
                Console.WriteLine($"Response: {response.StatusCode}");
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error deleting consult history after handled: {response.ReasonPhrase}");
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting consult history after handled: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteConsultHistoryBeforeHandledAsync(int consultHistoryId, DeleteConsultRequestDTO model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentException("Model must not be null", nameof(model));
                }
                var jsonContent = JsonSerializer.Serialize(model);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"ConsultHistory/delete-before-handled/{consultHistoryId}", content);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error deleting consult history before handled: {response.ReasonPhrase}");
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting consult history before handled: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<ConsultHistoryRequestClientDTO>> GetConsultHistoryRequestBySalesPersonAsync(string salesPersonId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"ConsultHistory/salesperson/{salesPersonId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<IEnumerable<ConsultHistoryRequestClientDTO>>();
                }
                else
                {
                    throw new Exception($"Error fetching consult history: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching consult history: {ex.Message}", ex);
            }
        }
    }
}