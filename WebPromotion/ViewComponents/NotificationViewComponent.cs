using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebPromotion.Business.Interface;
using WebPromotion.Models;
using WebPromotion.Services.DTO;
using WebPromotion.Services.Interface;

namespace WebPromotion.ViewComponents
{
    public class NotificationViewComponent : ViewComponent
    {
        private readonly INotificationBusiness _notificationBusiness;

        public NotificationViewComponent(INotificationBusiness notificationBusiness)
        {
            _notificationBusiness = notificationBusiness;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // ambil path page
            var path = ViewContext.HttpContext.Request.Path;

            if (path.ToString().StartsWith("/SalesPerson"))
            {
                var dealerId = 0;
                if (ViewBag.DealerId != null && !string.IsNullOrWhiteSpace(ViewBag.DealerId.ToString()))
                {
                    dealerId = Convert.ToInt32(ViewBag.DealerId);
                }
                Console.WriteLine($"NotificationViewComponent: Fetching notifications for dealer {ViewBag.DealerId}");
                var notifications = await _notificationBusiness.GetUnreadNotificationsBusiness(dealerId);
                return View(notifications);
            }

            return View(new List<NotificationDTO>());
        }
    }
}