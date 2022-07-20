using Microsoft.AspNetCore.Mvc;
using Portfolio.Models.LoginSystem;
using Portfolio.Data;
using Portfolio.Security;
using System.Linq;
using System;

namespace Portfolio.Controllers
{
    public class LoginSystemController : Controller
    {
        private readonly LoginSystemContext _db;
        public LoginSystemController(LoginSystemContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, ActionName(nameof(Login))]
        public IActionResult LoginComfirm(LoginViewModel model)
        {
            var GetUserFromDB = from u in _db.UserLogin where u.UserName == model.UserName select u;

            if (GetUserFromDB != null)
            {
                var UserData = GetUserFromDB.FirstOrDefault(u => u.UserName == model.UserName);

                string Password = ComputerToHash.StringToHash(model.UserPassword + UserData.Salt);

                if (Password == UserData.UserPassword)
                {
                    TempData["Login"] = "Login Successful";

                    return RedirectToAction("LoginSystemDisplayMessagePage", "LoginSystem");
                }
                else
                {
                    TempData["Login"] = "Password Error";

                    return RedirectToAction("LoginSystemDisplayMessagePage", "LoginSystem");
                }
            }
            else
            {
                TempData["Login"] = "User Name Error";

                return RedirectToAction("LoginSystemDisplayMessagePage", "LoginSystem");
            }           
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewData["Message"] = TempData["UserNameExist"];

            return View();
        }

        [HttpPost, ActionName(nameof(Register))]
        public IActionResult RegisterComfirm(RegisterViewModel model)
        {
            var GetUserFromDB = from u in _db.UserLogin where u.UserName == model.UserName select u;

            if (GetUserFromDB != null)
            {
                TempData["UserNameExist"] = "User name already exist";

                return RedirectToAction("Register", "LoginSystem");
            }

            UserLogin result = new UserLogin();
          
            result.UserName = model.UserName;
            result.Salt = Guid.NewGuid().ToString();
            result.UserPassword = ComputerToHash.StringToHash(model.UserPassword + result.Salt);
            result.UserEmail = model.UserEmail;
            
            _db.Add(result);
            _db.SaveChanges();

            TempData["Register"] = "Registration Sucessful";

            return RedirectToAction("LoginSystemDisplayMessagePage", "LoginSystem");
        }

        public IActionResult LoginSystemDisplayMessagePage()
        {
            ViewData["Message"] = TempData["Register"];
            ViewData["Message"] = TempData["Login"];

            return View();
        }

        //Ref : https://www.c-sharpcorner.com/blogs/remote-validation-in-mvc-5-using-remote-attribute
        //Ref : https://www.aspsnippets.com/Articles/ASPNet-Core-MVC-Check-Username-Availability-Exists-in-Database-using-AngularJS.aspx
        //Ref : https://www.yogihosting.com/instant-username-availability-check-feature/
        public JsonResult IsUserNameAvaiable(string UserName)
        {
            return Json(!_db.UserLogin.Any(u => u.UserName == UserName));
        }
    }
}
