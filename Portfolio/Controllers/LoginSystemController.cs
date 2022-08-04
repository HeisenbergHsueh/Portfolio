using Microsoft.AspNetCore.Mvc;
using Portfolio.Models.LoginSystem;
using Portfolio.Data;
using Portfolio.Security;
using System.Linq;
using System;
using System.Collections.Generic;

//cookie授權
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace Portfolio.Controllers
{
    public class LoginSystemController : Controller
    {
        private readonly PortfolioContext _db;
        public LoginSystemController(PortfolioContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region 登入
        /// <summary>
        /// 登入
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous] //任何人都可以瀏覽此頁面
        public IActionResult Login()
        {            
            return View();
        }

        [HttpPost, ActionName(nameof(Login))]
        [AllowAnonymous] //任何人都可以瀏覽此頁面
        [AutoValidateAntiforgeryToken]
        public IActionResult LoginComfirm(LoginViewModel model)
        {
            //確認有無此user的帳號
            var GetUserFromDB = from u in _db.UserLogin where u.UserName == model.UserName select u;

            if (GetUserFromDB != null)
            {
                //如果有則回傳該筆user的資料
                var UserData = GetUserFromDB.FirstOrDefault(u => u.UserName == model.UserName);

                //接著將key in的密碼進行hash比對
                string Password = ComputerToHash.StringToHash(model.UserPassword + UserData.Salt);

                if (Password == UserData.UserPassword)
                {
                    //使用Claims來做cookie驗證(製作通行證)
                    //有4個步驟要做
                    //(1)宣告一個Claim，用來設定要寫在通行證上的資訊，例如帳號、Email
                    //(2)宣告一個ClaimsIdentity，ClaimsIdentity表示一個身份，而這個身份是由很多個Claim所組成，
                    //   舉例來說 : 身分證、駕照都是屬於一個ClaimsI
                    //   dentity
                    //(3)宣告一個ClaimsPrincipal，ClaimsPrincipal指的就是證照的持有者，一個ClaimsPrincipal可以持有很多ClaimsIdentity
                    //(4)確定ClaimsPrincipal無誤之後，使用AuthenticationProperties為製作好的通行證，加上屬性，例如 : AllowRefresh、IsPersistent

                    //(1)
                    var Claims = new List<Claim>
                    {
                        //設定ClaimTypes.NameIdentifier，就是UserId
                        new Claim(ClaimTypes.NameIdentifier, UserData.UserId.ToString()),
                        //設定ClaimTypes.Name，就是使用者帳號
                        new Claim(ClaimTypes.Name, UserData.UserName),
                        //設定ClaimTypes.Email
                        new Claim(ClaimTypes.Email, UserData.UserEmail)
                        
                    };

                    //設定登入者所擁有的角色至Claim中
                    //(A).先從DB抓取角色清單
                    string[] RoleList = UserData.UserRole.Split(',');

                    //(B).使用foreach loop將擁有的角色一一加入到Claim
                    foreach(var Role in RoleList)
                    {
                        Claims.Add(new Claim(ClaimTypes.Role, Role));
                    }
                    
                    //(2)
                    var claimsIdentity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    //(3)
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    //(4)
                    var authProperties = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(5),
                        IsPersistent = false
                    };

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);
                  
                    //比對一致則登入成功
                    //TempData["Login"] = "Login Successful";

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //比對錯誤則顯示密碼錯誤的page
                    TempData["Login"] = "Password Error";

                    return RedirectToAction("LoginSystemDisplayMessagePage", "LoginSystem");
                }
            }
            else
            {
                //若找不到此user帳號，則秀User Name Error的錯誤
                TempData["Login"] = "User Name Error";

                return RedirectToAction("LoginSystemDisplayMessagePage", "LoginSystem");
            }           
        }
        #endregion

        #region 登出
        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region 註冊
        /// <summary>
        /// 註冊
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous] //任何人都可以瀏覽此頁面
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, ActionName(nameof(Register))]
        [AllowAnonymous] //任何人都可以瀏覽此頁面
        [AutoValidateAntiforgeryToken]
        public IActionResult RegisterComfirm(RegisterViewModel model)
        {
            UserLogin result = new UserLogin();
          
            result.UserName = model.UserName;
            result.Salt = Guid.NewGuid().ToString();
            result.UserPassword = ComputerToHash.StringToHash(model.UserPassword + result.Salt);
            result.UserEmail = model.UserEmail;
            result.UserRole = "User";
            
            _db.Add(result);
            _db.SaveChanges();

            TempData["Register"] = "Registration Sucessful";

            return RedirectToAction("LoginSystemDisplayMessagePage", "LoginSystem");
        }
        #endregion

        #region 設定不同權限的page
        public IActionResult LoginSystemDisplayMessagePage()
        {
            ViewData["Message"] = TempData["Register"];
            ViewData["Message"] = TempData["Login"];

            return View();
        }

        [Authorize]
        public IActionResult AuthPage()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        public IActionResult UserPage()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult AdministratorPage()
        {
            return View();
        }

        [Authorize(Roles = "User,Administrator")]
        public IActionResult MultipleRolePage()
        {
            return View();
        }
        #endregion

        #region 確認帳號是否可以註冊
        //Ref(1) : https://www.yogihosting.com/jquery-ajax-aspnet-core/
        //Ref(2) : https://www.aspsnippets.com/Articles/Check-Username-Availability-Exists-in-Database-using-jQuery-AJAX-in-ASPNet-MVC.aspx
        //Ref(3) : https://www.yogihosting.com/instant-username-availability-check-feature/
        /// <summary>
        /// 確認帳號是否已經被註冊過
        /// 使用jQeury的ajax，觸發後端的method，再回傳查詢的結果
        /// </summary>
        /// <param name="name">user端輸入的帳號</param>
        /// <returns></returns>
        public JsonResult CheckUserNameAvaiable(string name)
        {
            var SearchUserName = from u in _db.UserLogin where u.UserName == name select u;

            bool IsExist = false;

            if (SearchUserName.Count() > 0)
            {
                IsExist = true;
            }

            return Json(IsExist);
        }
        #endregion

        #region 重設密碼
        /// <summary>
        /// 重設密碼
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        private static bool ResetPassword()
        {
            return true;
        }

        [HttpPost, ActionName(nameof(ResetPasswordComfirm))]
        private static bool ResetPasswordComfirm()
        {
            return true;
        }
        #endregion

        #region 當權限不足時，會跳轉到Forbidden頁面
        public IActionResult Forbidden()
        {
            return View();
        }
        #endregion
    }
}
