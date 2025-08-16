using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebPromotion.Services.DTO;
using WebPromotion.Services.Interface;

namespace WebPromotion.Services
{
    public class NotificationServices : INotificationServices
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public NotificationServices(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["ApiBaseUrl"] ?? "");
        }

        public Task<List<NotificationDTO>> GetAllNotifications()
        {
            throw new NotImplementedException();
        }

        public async Task<List<NotificationDTO>> GetLimitNotification(int limit, int customId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Notification/GetLimitedNotifications/{limit}/{customId}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<NotificationDTO>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<NotificationDTO>();
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error fetching notifications", ex);
            }
        }

        public async Task<NotificationDTO> GetNotificationById(int id, string NotificationType)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Notification/GetNotificationById/{id}/{NotificationType}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<NotificationDTO>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new NotificationDTO();
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error fetching notification", ex);
            }
        }

        public async Task<List<NotificationDTO>> GetNotificationsBySearchClient(string searchTerm, int customId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Notification/GetNotificationBySearch/{searchTerm}/{customId}");
                Console.WriteLine($"GetNotificationsBySearchClient: Fetching notifications with search term '{searchTerm}' for customId {customId}");
                Console.WriteLine($"Response Status Code: {response}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<NotificationDTO>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<NotificationDTO>();  
                
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error fetching notifications by search", ex);
            }
        }

        public async Task<List<NotificationDTO>> GetUnreadNotificationsClient(int customId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Notification/GetUnreadNotifications/{customId}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<NotificationDTO>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<NotificationDTO>();
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error fetching unread notifications", ex);
            }
        }

        public async Task<List<NotificationDTO>> UpdateReadAllNotifications(bool isRead)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"Notification/UpdateReadAllNotifications", isRead);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<NotificationDTO>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<NotificationDTO>();
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error updating notifications", ex);
            }
        }

        public async Task<NotificationDTO> UpdateReadAndNotificationTypeClient(NotificationDTO notification, int salesPersonId)
        {
            try
            {
                var json = JsonSerializer.Serialize(notification);
                Console.WriteLine($"UpdateReadAndNotificationTypeClient: {json}");
                var response = await _httpClient.PutAsJsonAsync($"Notification/UpdateRead-and-NotificationType/{salesPersonId}", notification);

                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"UpdateReadAndNotificationTypeClient: Response status code: {response.StatusCode}");
                Console.WriteLine($"UpdateReadAndNotificationTypeClient: Response content: {content}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"API returned error {(int)response.StatusCode}: {content}");
                }

                if (!string.IsNullOrWhiteSpace(content))
                {
                    return JsonSerializer.Deserialize<NotificationDTO>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    return notification;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating notification type", ex);
            }
        }

        public async Task<NotificationDTO> UpdateReadNotificationStatus(int id, bool isRead)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"Notification/UpdateReadNotificationStatus/{id}", isRead);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<NotificationDTO>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error updating notification status", ex);
            }   
        }
    }
}