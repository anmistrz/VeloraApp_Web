using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebPromotion.ViewModels.Modal;

namespace WebPromotion.ViewComponents
{
    public class ModalConfirmationComponentViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ModalConfirmationViewModels model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "ModalConfirmationViewModels cannot be null");
            }

            return View(model);
        }
    }
}