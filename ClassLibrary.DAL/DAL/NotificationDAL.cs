using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.DAL.Interfaces;
using DealerApi.DAL.Context;
using DealerApi.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.DAL.DAL
{
    public class NotificationDAL : INotifications
    {
        private readonly DealerRndDBContext _context;

        public NotificationDAL(DealerRndDBContext context)
        {
            _context = context;
        }


        public Task<IEnumerable<Notification>> GetAllNotificationsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Notification>> GetLimitNotificationAsync(int limit)
        {
            try
            {
                return await _context.Notifications
                    .OrderByDescending(n => n.CreatedAt)
                    .Take(limit)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("Error fetching notifications", ex);
            }
        }

        public Task<Notification> GetNotificationByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Notification>> UpdateReadAllNotificationsAsync(bool isRead)
        {
            throw new NotImplementedException();
        }

        public Task<Notification> UpdateReadNotificationStatusAsync(int id, bool isRead)
        {
            throw new NotImplementedException();
        }
    }
}