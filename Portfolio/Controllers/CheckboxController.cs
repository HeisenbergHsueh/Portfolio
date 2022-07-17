using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Portfolio.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Portfolio.Models;

namespace Portfolio.Controllers
{
    public class CheckboxController : Controller
    {
        private readonly PortfolioContext _context;

        public CheckboxController(PortfolioContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            FruitModel model = new FruitModel();

            model.Fruits = LoadFruitItemFromDB(_context);

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(FruitModel model, string[] Fruit)
        {
            ViewBag.Message = "Select fruit items :\\n";

            model.Fruits = LoadFruitItemFromDB(_context);

            foreach (SelectListItem FruitItemCheck in model.Fruits)
            {
                //如果從前端的name回傳的string array中，有與model.Fruits相match的值
                //則將其SelectListItem的Selected改成true，然後將其value值新增到ViewBag.Message中
                if (Fruit.Contains(FruitItemCheck.Value))
                {
                    FruitItemCheck.Selected = true;
                    ViewBag.Message += string.Format("{0}\\n", FruitItemCheck.Text);
                }
            }

            return View(model);
        }

        /// <summary>
        /// 將DB中的水果清單載入到此Method中，最後再return一個fruit item的List
        /// </summary>
        /// <returns></returns>
        private static List<SelectListItem> LoadFruitItemFromDB(PortfolioContext context)
        {
            //(1). 從DB中找出所有的水果名稱與代號
            var FruitItemFromDB = from f in context.Fruits select f;

            //(2). 將這些水果名稱與代號放進下面create的list
            List<SelectListItem> FruitList = new List<SelectListItem>();

            foreach (var FruitItem in FruitItemFromDB)
            {
                FruitList.Add(new SelectListItem { 
                    Selected = false,
                    Value = FruitItem.FruitId.ToString(),
                    Text = FruitItem.FruitName
                });
            }         

            return FruitList;
        }
    }
}
