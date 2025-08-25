using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
using System.Threading.Tasks;
using WebPromotion.Services.DTO;
using WebPromotion.Services.Interface;

namespace WebPromotion.Services
{
    public class DashboardSalesPersonServices : IDashboardSalesPersonServices
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public DashboardSalesPersonServices(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            var apiBase = _configuration["ApiBaseUrl"];
            if (!string.IsNullOrWhiteSpace(apiBase))
            {
                _httpClient.BaseAddress = new Uri(apiBase);
            }
        }

        public Task<List<SalesActivityThisMonthClientDTO>> GetDetailActivitySalesPerformanceByDayInThisMonthAsync(int salesPersonId)
        {
            try
            {
                var response = _httpClient.GetAsync($"DashboardSalesPerson/detail-activity-sales-performance-by-day-in-this-month/{salesPersonId}");
                if (!response.Result.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException("Failed to fetch sales performance details by day");
                }
                var result = response.Result.Content.ReadAsStringAsync().Result;
                var dataJson = JsonSerializer.Deserialize<List<SalesActivityThisMonthClientDTO>>(result);
                if (dataJson == null)
                {
                    throw new InvalidOperationException("Failed to deserialize sales performance details data");
                }
                return Task.FromResult(dataJson);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred while fetching sales performance details: {ex.Message}", ex);
                return Task.FromResult(new List<SalesActivityThisMonthClientDTO>());
            }
        }

        public async Task<double> GetSalesPerformanceReviewAsync(int salesPersonId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"DashboardSalesPerson/sales-performance-review/{salesPersonId}");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"GetSalesPerformanceReviewAsync: API returned non-success status {response.StatusCode}");
                    return 0.0; // default value when API fails
                }

                var result = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(result))
                {
                    Console.WriteLine("GetSalesPerformanceReviewAsync: empty response content");
                    return 0.0;
                }

                // Try to parse plain numeric response
                if (double.TryParse(result, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedValue))
                {
                    return parsedValue;
                }

                // If API returns JSON number or object, attempt to deserialize
                try
                {
                    var jsonElement = JsonSerializer.Deserialize<System.Text.Json.JsonElement>(result);
                    if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.Number && jsonElement.TryGetDouble(out var jsonNumber))
                    {
                        return jsonNumber;
                    }

                    // If it's an object with a property named "value" or similar
                    if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.Object)
                    {
                        if (jsonElement.TryGetProperty("value", out var valueProp))
                        {
                            if (valueProp.ValueKind == System.Text.Json.JsonValueKind.Number && valueProp.TryGetDouble(out var valuePropDouble))
                            {
                                return valuePropDouble;
                            }
                        }
                    }
                }
                catch (Exception parseEx)
                {
                    Console.WriteLine($"GetSalesPerformanceReviewAsync: JSON parse attempt failed: {parseEx.Message}");
                }

                Console.WriteLine($"GetSalesPerformanceReviewAsync: Unable to parse response '{result}'");
                return 0.0;
            }
            catch (Exception ex)
            {
                // Log the exception and return a safe default
                Console.WriteLine($"GetSalesPerformanceReviewAsync: Exception while fetching sales performance review: {ex}");
                return 0.0;
            }
        }

        public async Task<double> GetSalesRatingAsync(int salesPersonId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"DashboardSalesPerson/sales-rating/{salesPersonId}");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"GetSalesRatingAsync: API returned non-success status {response.StatusCode}");
                    return 0.0; // safe default
                }

                var result = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(result))
                {
                    Console.WriteLine("GetSalesRatingAsync: empty response content");
                    return 0.0;
                }

                if (double.TryParse(result, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedValue))
                {
                    return parsedValue;
                }

                try
                {
                    var jsonElement = JsonSerializer.Deserialize<System.Text.Json.JsonElement>(result);
                    if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.Number && jsonElement.TryGetDouble(out var jsonNumber))
                    {
                        return jsonNumber;
                    }

                    if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.Object)
                    {
                        if (jsonElement.TryGetProperty("value", out var valueProp))
                        {
                            if (valueProp.ValueKind == System.Text.Json.JsonValueKind.Number && valueProp.TryGetDouble(out var valuePropDouble))
                            {
                                return valuePropDouble;
                            }
                        }
                    }
                }
                catch (Exception parseEx)
                {
                    Console.WriteLine($"GetSalesRatingAsync: JSON parse attempt failed: {parseEx.Message}");
                    return 0.0;
                }

                Console.WriteLine($"GetSalesRatingAsync: Unable to parse response '{result}'");
                return 0.0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetSalesRatingAsync: Exception while fetching sales rating: {ex}");
                return 0.0;
            }
        }

        public async Task<int> GetTotalConsultationHandledSalesPersonAsync(int salesPersonId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"DashboardSalesPerson/total-consultation-handled/{salesPersonId}");
                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException("Failed to fetch total consultations");
                }
                var result = await response.Content.ReadAsStringAsync();
                return int.Parse(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred while fetching total consultations handled: {ex.Message}", ex);
                return 0;
            }
        }

        public async Task<List<SalesActivitySummaryClientDTO>> GetTotalSalesActivityConsultationByMonthAsync(int salesPersonId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"DashboardSalesPerson/total-sales-activity-consultation-by-month/{salesPersonId}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to fetch total sales activity consultation by month: {response.ReasonPhrase}");
                    return [];
                }
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Result from API ByMonth: {result}");
                var dataJson = JsonSerializer.Deserialize<List<SalesActivitySummaryClientDTO>>(result);
                if (dataJson == null)
                {
                    Console.WriteLine("Failed to deserialize sales activity consultation data");
                    return [];
                }
                return dataJson;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred while fetching total sales activity consultation: {ex.Message}", ex);
                return [];
            }
        }

        public async Task<List<SalesActivitySummaryClientDTO>> GetTotalSalesActivityTestDriveByMonthAsync(int salesPersonId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"DashboardSalesPerson/total-sales-activity-test-drive-by-month/{salesPersonId}");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to fetch total sales activity test drive: {response.ReasonPhrase}");
                    return [];
                }
                var result = await response.Content.ReadAsStringAsync();
                var dataJson = JsonSerializer.Deserialize<List<SalesActivitySummaryClientDTO>>(result);
                if (dataJson == null)
                {
                    Console.WriteLine("Failed to deserialize sales activity test drive data");
                    return [];
                }
                return dataJson;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred while fetching total sales activity test drive: {ex.Message}", ex);
                return [];
            }
        }

        public async Task<List<SalesActivityPersonPerformanceClientDTO>> GetTotalTargetConsultationHandledSalesPersonAsync(int salesPersonId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"DashboardSalesPerson/total-target-consultation-handled/{salesPersonId}");
                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException("Failed to fetch total target consultation handled");
                }
                var result = await response.Content.ReadAsStringAsync();
                var dataJson = JsonSerializer.Deserialize<List<SalesActivityPersonPerformanceClientDTO>>(result);
                if (dataJson == null)
                {
                    throw new InvalidOperationException("Failed to deserialize total target consultation handled data");
                }
                return dataJson;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred while fetching total target consultation handled: {ex.Message}", ex);
                return [];
            }
        }

        public async Task<List<SalesActivityPersonPerformanceClientDTO>> GetTotalTargetTestDriveHandledSalesPersonAsync(int salesPersonId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"DashboardSalesPerson/total-target-test-drive-handled/{salesPersonId}");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to fetch total target test drive handled: {response.ReasonPhrase}");
                    return [];
                }
                var result = await response.Content.ReadAsStringAsync();
                var dataJson = JsonSerializer.Deserialize<List<SalesActivityPersonPerformanceClientDTO>>(result);
                if (dataJson == null)
                {
                    return [];
                }
                return dataJson;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred while fetching total target test drive handled: {ex.Message}", ex);
                return [];
            }
        }

        public async Task<int> GetTotalTestDriveHandledSalesPersonAsync(int salesPersonId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"DashboardSalesPerson/total-test-drive-handled/{salesPersonId}");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to fetch total test drive handled: {response.ReasonPhrase}");
                    return 0;
                }
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<int>(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred while fetching total test drive handled: {ex.Message}", ex);
                return 0;
            }
        }
    }
}