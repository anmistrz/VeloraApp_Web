using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPromotion.ViewModels.AccountView;

namespace WebPromotion.Business.Interface
{
    public interface IAccountBusiness
    {
        public Task<UserViewModel> LoginBusiness(LoginViewModel model);
        public Task<string> LogoutBusiness();
    }
}