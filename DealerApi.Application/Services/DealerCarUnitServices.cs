using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.DAL.Interfaces;
using DealerApi.Application.DTO;
using DealerApi.Application.Interface;
using DealerApi.Entities.Models;

namespace DealerApi.Application.Services
{
    public class DealerCarUnitServices : IDealerCarUnitServices
    {
        private readonly IDealerCarUnit _dealerCarUnitDAL;

        public DealerCarUnitServices (IDealerCarUnit dealerCarUnitDAL)
        {
            _dealerCarUnitDAL = dealerCarUnitDAL;
        }

        public async Task<DealerCarUnitDTO> CreateDealerCarUnit(DealerCarUnitDTO dealerCarUnit)
        {
            try
            {
                var body = new DealerCarUnit
                {
                    DealerCarUnitId = dealerCarUnit.DealerCarUnitId,
                    DealerCarId = dealerCarUnit.DealerCarId,
                    Vin = dealerCarUnit.Vin,
                    Color = dealerCarUnit.Color,
                    YearManufacture = dealerCarUnit.YearManufacture,
                    Status = dealerCarUnit.Status,
                    AddedDate = dealerCarUnit.AddedDate
                };
                var createdUnit = await _dealerCarUnitDAL.CreateAsync(body);
                return new DealerCarUnitDTO
                {
                    DealerCarUnitId = createdUnit.DealerCarUnitId,
                    DealerCarId = createdUnit.DealerCarId,
                    Vin = createdUnit.Vin,
                    Color = createdUnit.Color,
                    YearManufacture = createdUnit.YearManufacture,
                    Status = createdUnit.Status,
                    AddedDate = createdUnit.AddedDate
                };
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new ApplicationException("Error creating dealer car unit", ex);
            }
        }

        public async Task<bool> DeleteDealerCarUnit(int id)
        {
            try
            {
                return await _dealerCarUnitDAL.DeleteAsync(id);
            }
            catch (Exception e)
            {
                // Log the exception
                throw new ApplicationException("Error deleting dealer car unit", e);
            }
        }

        public async Task<int> DeleteDealerCarUnitWithConsultHistory(int dealerCarUnitId)
        {
            try
            {
                return await _dealerCarUnitDAL.DeleteDealerCarUnitWithConsultHistoryAsync(dealerCarUnitId);
            }
            catch (Exception e)
            {
                // Log the exception
                throw new ApplicationException("Error deleting dealer car unit with consult history", e);
            }
        }

        public async Task<IEnumerable<DealerCarUnitDTO>> GetAllDealerCarUnits()
        {
            try
            {
                var units = await _dealerCarUnitDAL.GetAllAsync();
                return units.Select(unit => new DealerCarUnitDTO
                {
                    DealerCarUnitId = unit.DealerCarUnitId,
                    DealerCarId = unit.DealerCarId,
                    Vin = unit.Vin,
                    Color = unit.Color,
                    YearManufacture = unit.YearManufacture,
                    Status = unit.Status,
                    AddedDate = unit.AddedDate
                });
            }
            catch (Exception e)
            {
                // Log the exception
                throw new ApplicationException("Error retrieving all dealer car units", e);
            }
        }

        public async Task<DealerCarUnitDTO> GetDealerCarUnitById(int id)
        {
            try
            {
                var unit = await _dealerCarUnitDAL.GetByIdAsync(id);
                return new DealerCarUnitDTO
                {
                    DealerCarUnitId = unit.DealerCarUnitId,
                    DealerCarId = unit.DealerCarId,
                    Vin = unit.Vin,
                    Color = unit.Color,
                    YearManufacture = unit.YearManufacture,
                    Status = unit.Status,
                    AddedDate = unit.AddedDate
                };
            }
            catch (Exception e)
            {
                // Log the exception
                throw new ApplicationException("Error retrieving dealer car unit by id", e);
            }
        }   

        public async Task<DealerCarUnitDTO> UpdateDealerCarUnit(DealerCarUnitDTO dealerCarUnit)
        {
            try
            {
                var updatedUnit = await _dealerCarUnitDAL.UpdateAsync(new DealerCarUnit
                {
                    DealerCarUnitId = dealerCarUnit.DealerCarUnitId,
                    DealerCarId = dealerCarUnit.DealerCarId,
                    Vin = dealerCarUnit.Vin,
                    Color = dealerCarUnit.Color,
                    YearManufacture = dealerCarUnit.YearManufacture,
                    Status = dealerCarUnit.Status,
                    AddedDate = dealerCarUnit.AddedDate
                });
                return new DealerCarUnitDTO
                {
                    DealerCarUnitId = updatedUnit.DealerCarUnitId,
                    DealerCarId = updatedUnit.DealerCarId,
                    Vin = updatedUnit.Vin,
                    Color = updatedUnit.Color,
                    YearManufacture = updatedUnit.YearManufacture,
                    Status = updatedUnit.Status,
                    AddedDate = updatedUnit.AddedDate
                };
            }
            catch (Exception e)
            {
                // Log the exception
                throw new ApplicationException("Error updating dealer car unit", e);
            }
        }
    }
}