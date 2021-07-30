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

        // GET: TesteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TesteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TesteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TesteController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TesteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TesteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TesteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet("view")]
        public ActionResult Testando()
        {
            ViewData["helloWorld"] = false;
            return View();
        }
    }
}
