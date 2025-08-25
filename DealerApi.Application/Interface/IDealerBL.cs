using DealerApi.Application.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerApi.Application.Interface
{
    public interface IDealerBL
    {
        public Task<IEnumerable<DealerOptionsDTO>> GetDealerOptions();
        public Task<IEnumerable<SelectListItem>> GetSelectListDealerItems();
        public Task<List<ListMostDealerDTO>> GetListMostDealerAsync(int top);
    }
}
