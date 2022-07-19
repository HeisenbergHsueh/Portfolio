using Microsoft.AspNetCore.Mvc;
using Portfolio.Models.LoginSystem;
using Portfolio.Data;
using Portfolio.Security;

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
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, ActionName(nameof(Register))]
        public IActionResult RegisterComfirm(RegisterViewModel model)
        {
            UserLogin result = new UserLogin();

            result.UserPassword = ComputerToHash.ComputerToMD5(model.UserPassword);

            return View();
        }
    }
}
