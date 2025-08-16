using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebPromotion.ViewModels.TestDriveView.TestDriveGuestViewModel
{
    public class TestDriveGuestViewModels
    {
        public int CarId { get; set; }
        public int DealerId { get; set; }
        public string DealerCarUnitId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime ConsultDate { get; set; }
        public string Note { get; set; }
    }
}