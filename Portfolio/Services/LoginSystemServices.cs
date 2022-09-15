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
        private readonly PortfolioContext _db;

        public LoginSystemServices(PortfolioContext db)
        {
            _db = db;
        }
        
        public UserLogin CheckUserDataByAccount(string UserAccount)
        {
            UserLogin UserData = QueryUserDataByAccount(UserAccount);

            return UserData;
        }

        private UserLogin QueryUserDataByAccount(string UserAccount)
        {
            UserLogin QueryUserDataResult = new UserLogin();

            QueryUserDataResult = (from u in _db.UserLogin where u.UserAccount == UserAccount select u).FirstOrDefault();

            //QueryUserDataResult = QueryUserData.FirstOrDefault(u => u.UserAccount == UserAccount);

            return QueryUserDataResult;
        }
    }
}
