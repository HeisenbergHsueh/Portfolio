using Microsoft.AspNetCore.Mvc;
using Portfolio.Models.LoginSystem;
using Portfolio.Data;
using Portfolio.Security;
using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;

//cookie授權
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

//寄信服務
using Microsoft.AspNetCore.Hosting;
using Portfolio.Services;

//組合驗證網址
using Microsoft.AspNetCore.Http;


namespace Portfolio.Controllers
{
    public class LoginSystemController : Controller
    {
        private readonly PortfolioContext _db;
        private readonly IWebHostEnvironment _env;

        public LoginSystemController(PortfolioContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
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
        public IActionResult Login(string returnUrl)
        {      
            //判斷是否已有帳號登入，如果有則導向首頁(Home/Index)，如果沒有則導向登入頁面(LoginSystem/Login)
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            TempData["returnUrl"] = returnUrl;

            return View();
        }

        /// <summary>
        /// 確認user輸入的帳密、設定cookie、權限
        /// </summary>
        /// <param name="model">User所輸入的帳號與密碼</param>
        /// <param name="returnUrl">User在登入前所在的頁面的url</param>
        /// <returns></returns>
        [HttpPost, ActionName(nameof(Login))]
        [AllowAnonymous] //任何人都可以瀏覽此頁面
        [ValidateAntiForgeryToken] //防止CSRF攻擊
        public IActionResult LoginComfirm(LoginViewModel model, string returnUrl)
        {
            LoginSystemServices LSS = new LoginSystemServices(_db);

            //確認有無此user的帳號
            //var GetUserFromDB = GetUserDataByAccount(model.UserAccount);
            var GetUserFromDB = LSS.CheckUserDataByAccount(model.UserAccount);

            if (GetUserFromDB != null)
            {
                ComputeToHash computeToHash = new ComputeToHash();

                //接著將key in的密碼進行hash比對
                string Password = computeToHash.StringToHash(String.Concat(model.UserPassword, GetUserFromDB.Salt), "MD5");

                if (Password == GetUserFromDB.UserPassword)
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
                        new Claim(ClaimTypes.NameIdentifier, GetUserFromDB.UserId.ToString()),
                        //設定ClaimTypes.Name，就是使用者帳號
                        new Claim(ClaimTypes.Name, GetUserFromDB.UserName),
                        //設定ClaimTypes.Email，就是登入者的Email address
                        new Claim(ClaimTypes.Email, GetUserFromDB.UserEmail)
                        
                    };

                    //設定登入者所擁有的角色至Claim中
                    //(A).先從DB抓取角色清單
                    string[] RoleList = GetUserFromDB.UserRole.Split(',');

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
                        //參考資料 : https://docs.microsoft.com/zh-tw/dotnet/api/microsoft.aspnetcore.authentication.authenticationproperties?view=aspnetcore-3.1
                        //IssuedUtc可用來設定通行證的開始時間，若沒有設定此參數，則預設為UtcNow
                        //IssuedUtc = DateTimeOffset.UtcNow,

                        //設定通行證的到期時間，若沒設定此選項，則通行證會吃到startup.cs中，AddCookie()所設定的expire date(60分鐘)
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                        //ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(20),
                        //參考資料 : https://ithelp.ithome.com.tw/articles/10000095
                        //IsPersistent = false，表示將cookie設定成暫時性的cookie，則在chrome瀏覽器上會顯示為session cookie
                        //session cookie會在使用者將瀏覽器關閉之後，將cookie的資訊清除
                        //因此，如果設定成暫時性的cookie的話，也就不需要設定ExpiresUtc
                        //IsPersistent = true，表示將cookie設定成持續性的cookie，即使在關閉瀏覽器之後，仍會繼續儲存，直到時間到期為止
                        IsPersistent = true,
                        //AllowRefresh : 讓使用者的通行證有效時間剩下一半時，可以透過重新整理刷新到期時間
                        //此Method與startup.cs中AddCookie()的SlidingExpiration一樣
                        AllowRefresh = true,
                        //URI、URL、URN(1) : https://www.796t.com/content/1541700250.html
                        //URI、URL、URN(2) : http://www.ifuun.com/a20179175334752/
                        //RedirectUri在允許用戶使用google、facebook、githug帳號進行登入的時候會用到，通常會使用像是oAuth 2.0這種方式來實作
                        //RedirectUri = "/"
                    };

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);

                    //以下的功能為 : 登入後自動跳轉回登入前的頁面
                    //舉例 : 當user嘗試進入A頁面，但因為A頁面需要授權，因此會先轉到登入頁面，但登入頁通常在登入完之後，
                    //都會預設去 /Home/Index 的頁面，而下面這段code，會讓user在登入後，直接跳轉至登入前的A頁面
                    //參考資料 : https://www.796t.com/content/1508336427.html
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //比對錯誤則顯示密碼錯誤的page
                    //ViewData、ViewBag、TempData的差異請參考 : https://www.796t.com/content/1548032961.html
                    //由於是固定的data type，因此使用ViewData
                    //而在此要注意的是，當使用ViewBag、ViewData時，不能使用RedirectToAction()
                    //因為重新定向之後，ViewBag、ViewData會變成null
                    //如果真的要使用RedirectToAction()，則必須使用TempData，TempData才能夠跨頁面使用
                    ViewData["ErrorMessage"] = "密碼錯誤";

                    return View("Login");
                }
            }
            else
            {
                //若找不到此user帳號，則秀User Name Error的錯誤
                ViewData["ErrorMessage"] = "查無此帳號";

                return View("Login");
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
            //判斷是否已有帳號登入，如果有則導向首頁(Home/Index)，如果沒有則導向登入頁面(LoginSystem/Login)
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost, ActionName(nameof(Register))]
        [AllowAnonymous] //任何人都可以瀏覽此頁面
        [ValidateAntiForgeryToken] //防止CSRF攻擊
        public IActionResult RegisterComfirm(RegisterViewModel model)
        {
            UserLogin result = new UserLogin();
            ComputeToHash computeToHash = new ComputeToHash();

            result.UserAccount = model.UserAccount;
            
            //隨機產生一組GUID當作salt
            result.Salt = Guid.NewGuid().ToString();
            //將password與salt合併之後，進行MD5的hash加密

            result.UserPassword = computeToHash.StringToHash(String.Concat(model.UserPassword,result.Salt), "MD5");


            result.UserName = model.UserName;
            result.UserEmail = model.UserEmail;
            //預設給註冊的帳號名為User的角色
            result.UserRole = "User";
            //註冊之Email是否已驗證過，1表示還沒驗證，0則表示已經驗證
            result.IsEmailAuthenticated = 1;
            //將註冊帳號以MD5加密之hash，並作為後續User驗證信箱用的驗證碼
            result.AuthCode = computeToHash.StringToHash(model.UserAccount, "MD5");

            //讀取 WebRootPath(就是wwwroot所在目錄)
            string WebRootPath = _env.WebRootPath.ToString();

            //組合驗證Email的驗證網址
            string EmailValidationURL = $"{Request.Scheme}://{Request.Host.Value}/api/WebAPI/{result.UserAccount}/{result.AuthCode}";

            //讀取信件範本
            string MailTemplate = System.IO.File.ReadAllText(WebRootPath + @"\Mail\RegisterEmailTemplate.html");
            //將信件範本中的{{UserName}}、{{EmailValidationURL}}分別用result.UserName、EmailValidationURL取代
            MailTemplate = MailTemplate.Replace("{{UserName}}", result.UserName);
            MailTemplate = MailTemplate.Replace("{{EmailValidationURL}}", EmailValidationURL);

            //寄送驗證信
            MailServices mailService = new MailServices();
            mailService.SendMail(result.UserEmail, WebRootPath, MailTemplate);

            //將註冊資料存入資料庫中
            _db.Add(result);
            _db.SaveChanges();

            TempData["Message"] = "會員註冊成功，請至註冊時所填的信箱收取驗證信進行驗證";

            return RedirectToAction("LoginSystemDisplayMessagePage", "LoginSystem");
        }
        #endregion

        #region 設定不同權限的page
        public IActionResult LoginSystemDisplayMessagePage()
        {
            ViewData["Message"] = TempData["Message"];

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

        [Authorize(Policy = "IsIT")]
        public IActionResult IT_could_be_browser_Page()
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
            //var SearchUserName = from u in _db.UserLogin where u.UserAccount == name select u;

            LoginSystemServices LSS = new LoginSystemServices(_db);

            var SearchUserName = LSS.CheckUserDataByAccount(name);

            bool IsExist = false;

            if (SearchUserName != null)
            {
                IsExist = true;
            }

            return Json(IsExist);
        }
        #endregion

        #region 重設密碼
        [Authorize] //必須要登入帳號才可以使用重設密碼的功能
        public IActionResult ResetPassword()
        {
            

            return View();
        }

        [HttpPost, ActionName(nameof(ResetPasswordComfirm))]
        public IActionResult ResetPasswordComfirm()
        {
            return View();
        }

        /// <summary>
        /// 確認舊密碼是否輸入正確
        /// </summary>
        /// <returns></returns>
        private bool IsCheckOldPasswordCorrect(string PwdHash)
        {
            return true;
        }

        /// <summary>
        /// 將已轉成hash的新密碼update到DB
        /// </summary>
        /// <returns></returns>
        private bool UpdateNewPasswordToDB(string PwdHash)
        {
            return true;
        }


        #endregion

        #region 忘記密碼
        #endregion

        #region 當權限不足時，會跳轉到Forbidden頁面
        public IActionResult Forbidden()
        {
            return View();
        }
        #endregion

        #region 查詢單筆UserData
        /// <summary>
        /// 利用User Account來查詢資料庫中，是否有該筆帳號的資料
        /// </summary>
        /// <param name="UserAccount">使用者帳號</param>
        /// <returns></returns>
        public UserLogin GetUserDataByAccount(string UserAccount)
        {
            UserLogin UserData = new UserLogin();

            var QueryUserData = from u in _db.UserLogin where u.UserAccount == UserAccount select u;

            if (QueryUserData != null)
            {
                UserData = QueryUserData.FirstOrDefault(u => u.UserAccount == UserAccount);

                return UserData;
            }

            return null;
        }
        #endregion
       
    }
}
