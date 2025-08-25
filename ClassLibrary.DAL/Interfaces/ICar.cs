using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.DAL.Interfaces;
using DealerApi.Entities.Models;

namespace ClassLibrary.DAL.Interfaces
{
    public interface ICar : ICrud<Car>
    {
        public Task<List<(Car, Dealer, DealerCar, int totalCount)>> GetMostTestDriveCarsWithDealersAsync(int top);
    }
}