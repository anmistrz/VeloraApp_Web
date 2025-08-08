using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebPromotion.ViewModels.AccountView
{
    public class LoginViewModel
    {
        //email
        public string email { get; set; }

        //password
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}