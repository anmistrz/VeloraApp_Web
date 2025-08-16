using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DealerApi.Application.DTO;
using WebPromotion.Services.DTO;

namespace WebPromotion.Services.TestDrive
{
    public class TestDriveServices : ITestDriveService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public TestDriveServices(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["ApiBaseUrl"] ?? "");
        }

        public Models.TestDrive Create(Models.TestDrive entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Models.TestDrive> CreateAsyncTestDriveGuest(TestDriveInsertGuestDTO model)
        {
           try 
           {
                var response = await _httpClient.PostAsJsonAsync("TestDrive/create-guest", model);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Models.TestDrive>();
                }
                else
                {
                    throw new Exception($"Error creating test drive: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the test drive guest.", ex);
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteTestDriveAfterHandledAsync(int consultHistoryId, DeleteTestDriveRequestDTO model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentException("Model must not be null", nameof(model));
                }
                var jsonContent = JsonSerializer.Serialize(model);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"TestDrive/delete-after-handled/{consultHistoryId}", content);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error deleting test drive after handled: {response.ReasonPhrase}");
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error deleting test drive after handled", ex);
            }
        }

        public Task<bool> DeleteTestDriveBeforeHandledAsync(int consultHistoryId, DeleteTestDriveRequestDTO model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentException("Model must not be null", nameof(model));
                }
                var jsonContent = JsonSerializer.Serialize(model);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync($"TestDrive/delete-before-handled/{consultHistoryId}", content).Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error deleting test drive before handled: {response.ReasonPhrase}");
                }
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error deleting test drive before handled", ex);
            }
        }

        public IEnumerable<Models.TestDrive> GetAll()
        {
            throw new NotImplementedException();
        }

        public Models.TestDrive GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TestDriveRequestClientDTO>> GetTestDriveRequestBySalesPersonAsync(string salesPersonId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"TestDrive/by-salesperson/{salesPersonId}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<IEnumerable<TestDriveRequestClientDTO>>();
                    return result;
                }
                else
                {
                    throw new Exception($"Error fetching test drive requests: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching test drive requests: {ex.Message}", ex);
            }
        }

        public Models.TestDrive Update(Models.TestDrive entity)
        {
            throw new NotImplementedException();
        }
    }
}