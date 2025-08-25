using ClassLibrary.DAL.Interfaces;
using DealerApi.Application.DTO;
using DealerApi.Application.Interface;
using DealerApi.DAL;
using DealerApi.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerApi.Application
{
    public class DealerBL : IDealerBL
    {
        private readonly IDealer _dealerDAL;

        public DealerBL(IDealer dealerDAL)
        {
            _dealerDAL = dealerDAL;
        }

        public async Task<IEnumerable<DealerOptionsDTO>> GetDealerOptions()
        {
            try
            {
                var dealers = await _dealerDAL.GetAllAsync(); // Properly await the async call
                return dealers.Select(dealer => new DealerOptionsDTO
                {
                    DealerID = dealer.DealerId,
                    DealerName = dealer.DealerName
                });
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new InvalidOperationException("Error retrieving dealer options", ex);
            }
        }

        public async Task<List<ListMostDealerDTO>> GetListMostDealerAsync(int top)
        {
            try
            {
                var result = await _dealerDAL.GetMostPopularDealersAsync(top);
                var dataConvert = result.ToList();
                var convertDTO = dataConvert.Select(item => new ListMostDealerDTO
                {
                    DealerId = item.Item1.DealerId,
                    DealerName = item.Item1.DealerName,
                    Location = item.Item1.Location,
                    Province = item.Item1.Province,
                    City = item.Item1.City,
                    Address = item.Item1.Address,
                    PhoneNumber = item.Item1.PhoneNumber,
                    Email = item.Item1.Email,
                    DealerRating = item.Item2
                }).ToList();

                return convertDTO;
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new InvalidOperationException("Error retrieving most popular dealers", ex);
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetSelectListDealerItems()
        {
            try
            {
                var dealers = await _dealerDAL.GetAllAsync(); // Properly await the async call
                return dealers.Select(dealer => new SelectListItem
                {
                    Value = dealer.DealerId.ToString(),
                    Text = dealer.DealerName
                });
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new InvalidOperationException("Error retrieving dealer select list items", ex);
            }
        }
    }
}
