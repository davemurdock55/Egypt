using Egypt.Data;
using Egypt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Egypt.Controllers
{
    public class MummyController : Controller
    {
        private ILogger<MummyController> _logger;

        private mummiesContext context { get; set; }

        public MummyController(ILogger<MummyController> logger, mummiesContext temp_context)
        {
            _logger = logger;
            context = temp_context;

        }

        [HttpGet]
        public IActionResult BurialEntry()
        {
            return View();
        }
        [HttpPost]
        public IActionResult BurialEntry(Burialmain ar)
        {
            if (ModelState.IsValid)
            {
                ar.Id = context.Burialmain.OrderBy(x => x.Id).Last().Id + 1;
                context.Add(ar);
                context.SaveChanges();
                return RedirectToAction("Burials", "Home", ar);
            }
            else
            {

                return View(ar);
            }
        }

        [HttpGet]
        public IActionResult Edit(long deadid)
        {

            var entry = context.Entries.Single(x => x.Id == deadid);

            return View("EditPage", entry);
        }

        [HttpPost]
        public IActionResult Edit(Burialmain mum)
        {
            context.Update(mum);
            context.SaveChanges();
            return RedirectToAction("Burials", "Home");
        }

        [HttpGet]
        public IActionResult Delete(long deadid)
        {
            var entry = context.Entries.Single(x => x.Id == deadid);
            return View(entry);
        }
        [HttpPost]
        public IActionResult Delete(Burialmain ar)
        {
            context.Entries.Remove(ar);
            context.SaveChanges();

            return RedirectToAction("Burials", "Home");
        }
    }
}