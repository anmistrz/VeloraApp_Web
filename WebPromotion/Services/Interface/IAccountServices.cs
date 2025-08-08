using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPromotion.Services.DTO;
using WebPromotion.ViewModels.AccountView;

namespace WebPromotion.Services.Interface
{
    public interface IAccountServices
    {
        Task<UserViewModel> LoginAsync(LoginDTO model);
    }
}