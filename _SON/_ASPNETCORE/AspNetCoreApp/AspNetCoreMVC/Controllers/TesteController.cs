using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMVC.Controllers
{
    public class TesteController : Controller
    {
        // GET: TesteController
        public ActionResult Index()
        {
            return View();
        }

		public IActionResult AnotherAction()
		{
			return View();
		}

		public IActionResult AnotherView()
		{
			return View("SomeView");
		}

		public IActionResult ViewWithParameter()
		{
			string SomeString = "I am a string";

			return View("ViewWithParameter", SomeString);
		}

		public IActionResult PassingData()
		{
			ViewBag.Fruit = "Apples";
			ViewData["Color"] = "Red";

			TempData["Number"] = 5;

			return View();
		}

		// localhost:59025/home/QueryString?name=Jonh&lastname=Jonhson
		public IActionResult QueryString(string name, string lastname)
		{
			ViewBag.Name = name;

			ViewBag.Lastname = lastname;

			return View();
		}

		public RedirectResult Redirect2()
		{
			return Redirect("http://www.google.com");
		}

		// Redirecionando para o outro controller com nome da Action e nome do Controller
		public IActionResult Redirect3()
		{
			return RedirectToAction("PassingData", "AnotherController");
		}

		// Redirecionando para o outro controller passando um id
		public IActionResult Redirect4()
		{
			return RedirectToAction("PassingData", "AnotherController", new { id = 1 });
		}

		public IActionResult Redirect()
		{
			return RedirectToAction("PassingData");
		}

		[HttpGet("view")]
        public ActionResult Testando()
        {
            ViewData["helloWorld"] = false;
            return View();
        }
    }
}
