using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Controllers
{
    public class OnsiteCaseSystemController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
