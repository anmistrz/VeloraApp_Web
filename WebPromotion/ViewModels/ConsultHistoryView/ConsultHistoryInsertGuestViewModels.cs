using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebPromotion.ViewModels.ConsultHistoryView
{
    public class ConsultHistoryInsertGuestViewModels
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Please select a dealer")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a dealer")]
        public int DealerId { get; set; }

        public string DealerCarUnitId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string? FirstName { get; set; }
        
        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string? Email { get; set; }
        
        [Required(ErrorMessage = "Phone number is required")]
        public string? PhoneNumber { get; set; }

        public int SalesPersonId { get; set; }

        public decimal? Budget { get; set; }

        public DateTime ConsultDate { get; set; }

        public string? Note { get; set; }

        public bool? isOnInsert { get; set; } = false;
    }
}