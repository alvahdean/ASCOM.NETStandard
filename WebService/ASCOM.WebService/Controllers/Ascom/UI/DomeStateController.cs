using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASCOM.WebService.Controllers
{
    [Route("rascom/ui/[controller]")]
    public class DomeStateController : Controller
    {
        // GET: DomeState
        public ActionResult Index()
        {
            return View();
        }

        // GET: DomeState/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DomeState/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DomeState/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DomeState/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DomeState/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DomeState/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DomeState/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}