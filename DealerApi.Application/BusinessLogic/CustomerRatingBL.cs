using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.DAL.Interfaces;
using DealerApi.Application.DTO;
using DealerApi.Application.Interface;
using DealerApi.Entities.Models;

namespace DealerApi.Application.BusinessLogic
{
    public class CustomerRatingBL : ICustomerRatingBL
    {
        private readonly ICustomerRating _customerRatingDAL;

        public CustomerRatingBL(ICustomerRating customerRatingDAL)
        {
            _customerRatingDAL = customerRatingDAL;
        }

        public async Task<CustomerRatingDTO> CreateCustomerRatingAsync(CustomerRatingDTO customerRating)
        {
            try
            {
                var entity = new CustomerRating
                {
                    CustomerId = customerRating.CustomerId,
                    SalesPersonId = customerRating.SalesPersonId,
                    DealerId = customerRating.DealerId,
                    ConsultHistoryId = customerRating.ConsultHistoryId,
                    TestDriveId = customerRating.TestDriveId,
                    RatingType = customerRating.RatingType,
                    RatingValue = customerRating.RatingValue,
                    Comments = customerRating.Comments
                };

                await _customerRatingDAL.CreateAsync(entity);

                return customerRating;
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new Exception("An error occurred while creating the customer rating.", ex);
            }
        }
    }
}