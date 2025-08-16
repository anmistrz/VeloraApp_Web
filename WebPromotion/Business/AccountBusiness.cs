using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPromotion.Business.Interface;
using WebPromotion.Services.DTO;
using WebPromotion.Services.Interface;
using WebPromotion.ViewModels.AccountView;

namespace WebPromotion.Business
{   
    public class AccountBusiness : IAccountBusiness
    {
        private readonly IAccountServices _accountServices;

        public AccountBusiness(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }


        public async Task<UserViewModel> LoginBusiness(LoginViewModel model)
        {
            try
            {
                var body = new LoginDTO
                {
                    email = model.email,
                    password = model.password
                };
                var user = await _accountServices.LoginAsync(body);
                if (user != null)
                {
                    return new UserViewModel
                    {
                        Email = user.Email,
                        Token = user.Token // Assuming Token is not used in the response
                    };
                }
                else
                {
                    // Handle login failure (e.g., return null or throw an exception)
                    return null;

                }
            }
            catch (Exception ex)
            {
               return null; // Handle the exception as needed, e.g., log it or rethrow
                // Log the exception (not implemented here)
            }
        }

        public async Task<string> LogoutBusiness()
        {
            try
            {
                return await _accountServices.LogoutAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("Logout failed", ex);
            }
        }
    }
}