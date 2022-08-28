using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Portfolio.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

//hash加密
using Portfolio.Security;

//cookie授權
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Portfolio.Services;

namespace Portfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //string wwwroot_path = _env.WebRootPath.ToString();
            //Console.WriteLine(wwwroot_path);

            //MailService mailService = new MailService();

            //mailService.SendMail("nonsensehao@gmail.com");

            //string AuthCode = mailService.GenerateAuthCode();

            //Console.WriteLine(AuthCode);

            return View();
        }

        [Authorize]
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
