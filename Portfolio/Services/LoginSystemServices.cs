using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Portfolio.Models.LoginSystem;
using Portfolio.Data;

namespace Portfolio.Services
{
    public class LoginSystemServices
    {
        private readonly PortfolioContext db = new PortfolioContext();
        
        private UserLogin GetUserDataByAccount(string UserAccount)
        {
            UserLogin UserData = new UserLogin();

            return UserData;
        }
    }
}
