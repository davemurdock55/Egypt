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
                context.Add(ar);
                context.SaveChanges();
                return View("ConfirmationOfAdd", ar);
            }
            else
            {

                return View(ar);
            }
        }
    }
}
