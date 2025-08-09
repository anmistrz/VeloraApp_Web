using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.DAL.Interfaces;
using DealerApi.Application.DTO;
using DealerApi.Application.Interface;

namespace DealerApi.Application.Services
{
    public class NotificationServices : INotificationServices
    {
        private readonly INotifications _notificationDAL;
        public NotificationServices(INotifications notificationDAL)
        {
            _notificationDAL = notificationDAL;
        }


        public List<NotificationDTO> GetAllNotifications()
        {
            throw new NotImplementedException();
        }

        public List<NotificationDTO> GetLimitNotification(int limit)
        {
            try
            {
                var notifications = _notificationDAL.GetLimitNotificationAsync(limit).Result;
                Console.WriteLine($"Fetched {notifications.Count()} notifications with limit {limit}");
                var result = notifications.Select(n => new NotificationDTO
                {
                    NotificationId = n.NotificationId,
                    SalesPersonId = n.SalesPersonId,
                    CustomerId = n.CustomerId,
                    ConsultHistoryId = n.ConsultHistoryId,
                    TestDriveId = n.TestDriveId,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    Priority = n.Priority,
                    DealerId = n.DealerId
                }).ToList();
                
                return result;
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error fetching limited notifications", ex);
            }
        }

        public NotificationDTO GetNotificationById(int id)
        {
            throw new NotImplementedException();
        }

        public List<NotificationDTO> UpdateReadAllNotifications(bool isRead)
        {
            throw new NotImplementedException();
        }

        public NotificationDTO UpdateReadNotificationStatus(int id, bool isRead)
        {
            throw new NotImplementedException();
        }
    }
}