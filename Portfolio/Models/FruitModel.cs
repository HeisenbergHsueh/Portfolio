using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Portfolio.Models
{
    public class FruitModel
    {
        public List<SelectListItem> Fruits { get; set; }
    }
}
