using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealerApi.Application.DTO
{
    public class SalesPersonDTO
    {
        public int SalesPersonId { get; set; }

        public int DealerId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int? Bonus { get; set; }

        public bool IsActive { get; set; }
    }
}