using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Razor.Models;

namespace Razor.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
			Product bananas = new Product
			{
				ID = 1,
				Name = "Bananas",
				Desc = "Nice bananas",
				Price = 1.50M
			};

			ViewBag.Qty = 4;

            return View(bananas);
        }

		public IActionResult Collection()
		{
			Product[] products =
			{
				new Product { Name = "Bananas", Price = 1.40M },
				new Product { Name = "Apples", Price = 1.00M },
				new Product { Name = "Oranges", Price = 1.20M },
				new Product { Name = "Kiwis", Price = 1.70M },
			};
			
			return View(products);
		}
	}
}
