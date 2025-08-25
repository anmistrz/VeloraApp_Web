using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.Application.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DealerApi.Application.Interface
{
    public interface ICarBL
    {
        public Task<IEnumerable<CarOptionsDTO>> GetCarOptions();
        public Task<IEnumerable<ListMostCarTestDriveDTO>> ListMostCarTestDrivesTop(int top);
        public Task<IEnumerable<SelectListItem>> GetCarSelectListItems();

    }
}