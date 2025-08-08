using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using WebPromotion.Services.DTO;
using WebPromotion.Services.Interface;
using WebPromotion.ViewModels.AccountView;

namespace WebPromotion.Services
{
    public class AccountService : IAccountServices
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AccountService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["ApiBaseUrl"]);
        }


        public async Task<UserViewModel> LoginAsync(LoginDTO model)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(model);
                var httpContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("userAuth/login", httpContent);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var userViewModel = JsonSerializer.Deserialize<UserViewModel>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (userViewModel == null)
                    {
                        throw new Exception("Failed to deserialize user data.");
                    }

                    return userViewModel;
                }
                else
                {
                    // Handle error response
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Login failed: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("Login failed", ex);
            }
        }
    }
}