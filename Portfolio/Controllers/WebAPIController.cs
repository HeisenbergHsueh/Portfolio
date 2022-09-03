using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

using Portfolio.Data;


// 參考資料(1) : https://ithelp.ithome.com.tw/users/20107452/ironman/4560
// 參考資料(2) : https://www.huanlintalk.com/2013/01/aspnet-web-api-parameter-binding.html
// 參考資料(3) : https://blog.yowko.com/aspdotnet-core-controller-controllerbase/


namespace Portfolio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebAPIController : Controller
    {
        //建構子
        private readonly PortfolioContext _db;

        public WebAPIController(PortfolioContext db)
        {
            _db = db;
        }

        // GET: api/<WebAPIController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        #region 驗證信箱的API
        [HttpGet("{UserAccount}/{AuthCode}")]
        public IActionResult EmailValidation(string UserAccount, string AuthCode)
        {
            //確認有無此user的帳號
            var GetUserFromDB = from u in _db.UserLogin where u.UserAccount == UserAccount select u;

            if (GetUserFromDB != null)
            {
                //如果有則回傳該筆user的資料
                var UserData = GetUserFromDB.FirstOrDefault(u => u.UserAccount == UserAccount);

                if (UserData.AuthCode == AuthCode)
                {
                    if (UserData.IsEmailAuthenticated != 0)
                    {
                        //若驗證碼比對成功，則將DB中的IsEmailAuthenticated由1(false)改成0(True)
                        UserData.IsEmailAuthenticated = 0;

                        //然後使用Update，更新UserData的資料，然後再存回DB
                        _db.Update(UserData);
                        _db.SaveChanges();

                        TempData["Message"] = "Authentication Sucessful";

                        return RedirectToAction("LoginSystemDisplayMessagePage", "LoginSystem");
                    }
                    else
                    {
                        TempData["Message"] = "This account was authenticated";

                        return RedirectToAction("LoginSystemDisplayMessagePage", "LoginSystem");
                    }

                }
                else
                {
                    TempData["Message"] = "Authentication Failed......";

                    return RedirectToAction("LoginSystemDisplayMessagePage", "LoginSystem");
                }
            }
            else
            {
                //如果找不到該筆user資料，則跳轉到顯示error message的頁面
                TempData["Message"] = "查無此帳號";
                return RedirectToAction("LoginSystemDisplayMessagePage", "LoginSystem");
            }
        }
        #endregion
    }
}
