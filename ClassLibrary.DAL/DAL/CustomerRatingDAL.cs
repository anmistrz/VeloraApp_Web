using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.DAL.Interfaces;
using DealerApi.DAL.Context;
using DealerApi.Entities.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.DAL.DAL
{
    public class CustomerRatingDAL : ICustomerRating
    {
        private readonly DealerRndDBContext _context;

        public CustomerRatingDAL(DealerRndDBContext context)
        {
            _context = context;
        }

        public async Task<CustomerRating> CreateAsync(CustomerRating entity)
        {
            try
            {
                var sql = "EXEC sp_InsertCustomerRatingWithTransaction @CustomerId, @SalesPersonId, @RatingType, @ConsultHistoryId, @TestDriveId, @RatingValue, @DealerId, @Comments";
                var parameters = new[]
                {
                    new SqlParameter("@CustomerId", entity.CustomerId),
                    new SqlParameter("@SalesPersonId", entity.SalesPersonId.HasValue ? (object)entity.SalesPersonId.Value : DBNull.Value),
                    new SqlParameter("@RatingType", System.Data.SqlDbType.NVarChar, 50) { Value = entity.RatingType ?? (object)DBNull.Value },
                    new SqlParameter("@ConsultHistoryId", entity.ConsultHistoryId.HasValue ? (object)entity.ConsultHistoryId.Value : DBNull.Value),
                    new SqlParameter("@TestDriveId", entity.TestDriveId.HasValue ? (object)entity.TestDriveId.Value : DBNull.Value),
                    new SqlParameter("@RatingValue", Convert.ToInt32(entity.RatingValue)),
                    new SqlParameter("@DealerId", entity.DealerId.HasValue ? (object)entity.DealerId.Value : DBNull.Value),
                    new SqlParameter("@Comments", System.Data.SqlDbType.NVarChar, -1) { Value = entity.Comments ?? (object)DBNull.Value },
                };

                await _context.Database.ExecuteSqlRawAsync(sql, parameters);
                return entity;
            }
            catch (Exception ex)
            {
                var errorMessage = $"An error occurred while creating the customer rating.\nMessage: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\nInner Exception: {ex.InnerException.Message}";
                }
                throw new Exception(errorMessage, ex);
            }
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CustomerRating>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CustomerRating> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CustomerRating> UpdateAsync(CustomerRating entity)
        {
            throw new NotImplementedException();
        }
    }
}