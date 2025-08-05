﻿using DealerApi.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerApi.Application.Interface
{
    public interface IDealerService
    {
        public Task<IEnumerable<DealerOptionsDTO>> GetDealerOptions();
    }
}
