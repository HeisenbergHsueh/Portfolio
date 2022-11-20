using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Portfolio.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

//hash加密
using Portfolio.Security;

//cookie授權
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Portfolio.Services;

//
using Microsoft.AspNetCore.Http.Extensions;

namespace Portfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }


        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Index()
        {
            #region 功能測試區
            //string wwwroot_path = _env.WebRootPath.ToString();
            //Console.WriteLine(wwwroot_path);

            //MailService mailService = new MailService();

            //mailService.SendMail("");

            //string AuthCode = mailService.GenerateAuthCode();

            //Console.WriteLine(AuthCode);

            //string UserAccount = "User06";

            //string AuthCode = "0FF134D04A4DE30211BC0EE0D84251EA";

            //string EmailValidationURL = $"{Request.Scheme}://{Request.Host.Value}/api/WebAPI/{UserAccount}/{AuthCode}";

            //Console.WriteLine("test");

            //string PublicKey = _config.GetConnectionString("Public_Key").ToString();

            //CipherServices CS = new CipherServices();

            //string EncryptConnectionString = CS.PlainTextToCipher("", PublicKey);
            #endregion

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
