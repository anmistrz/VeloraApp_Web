using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.Entities.Models;
using DealerApi.DAL.Interfaces;

namespace ClassLibrary.DAL.Interfaces
{
    public interface IDealer : ICrud<Dealer>
    {
        Task<IEnumerable<Dealer>> GetBySearchAsync(string searchTerm);
        Task<IEnumerable<(Dealer, float average)>> GetMostPopularDealersAsync(int top);
    }
}