using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerApi.Entities.Models;

namespace DealerApi.Application.DTO
{
    public class ListMostCarTestDriveDTO
    {

        public int carId { get; set; }
        public string make { get; set; }
        public string carModel { get; set; }
        public string carType { get; set; }
        public int? year { get; set; }
        public string engineSize { get; set; }
        public string fuelType { get; set; }
        public string transmission { get; set; }
        public string color { get; set; }
        public string description { get; set; }
        public DealerCustomDTO dealerDetail { get; set; }
        public DealerCarCustomDTO dealerCarDetail { get; set; }
        public int TestDriveCount { get; set; }
    }
}

public class DealerCustomDTO
{
    public int dealerId { get; set; }
    public string dealerName { get; set; }
    public string dealerLocation { get; set; }
    public string dealerProvince { get; set; }
    public string dealerCity { get; set; }
    public string dealerAddress { get; set; }
    public string dealerContact { get; set; }
    public string dealerEmail { get; set; }
}

public class DealerCarCustomDTO
{
    public int dealerCarId { get; set; }
    public int stock { get; set; }
    public double dealerPrice { get; set; }
    public double tax { get; set; }
    public string status { get; set; }
}