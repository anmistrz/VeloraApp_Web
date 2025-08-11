using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebPromotion.ViewModels.Modal
{
    public class ModalConfirmationViewModels
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string ConfirmButtonText { get; set; }
        public string CancelButtonText { get; set; }
        public string ConfirmButtonClass { get; set; } = "btn-primary";
        public string CancelButtonClass { get; set; } = "btn-secondary";
        public string ModalId { get; set; } = "confirmationModal";
        public bool IsVisible { get; set; } = false;
    }
}