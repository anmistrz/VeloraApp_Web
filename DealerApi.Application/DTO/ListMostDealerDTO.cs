using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.Entities.Models;

//   final String dealerId;
//   final String dealerName;
//   final double dealerRating;
//   final String location;
//   final String province;
//   final String dealerImage;
//   final String city;
//   final String address;
//   final String phoneNumber;
//   final String email;

namespace DealerApi.Application.DTO
{
    public class ListMostDealerDTO
    {
        public int DealerId { get; set; }

        public string DealerName { get; set; }

        public string Location { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
        public double DealerRating { get; set; }
    }
}