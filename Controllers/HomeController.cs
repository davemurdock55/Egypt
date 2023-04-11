using Egypt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Egypt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private mummiesContext context { get; set; }

        public HomeController(ILogger<HomeController> logger, mummiesContext temp_context)
        {
            _logger = logger;
            context = temp_context;
        }

        public IActionResult Index()
        {
            //var books = context.Books.OrderBy(x => x.Title).ToList();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult BurialEntry()
        {
            return View();
        }
        [HttpPost]
        public IActionResult BurialEntry(Burialmain bm)
        {
            return View("ConfirmationOfAdd", bm);
        }
        public IActionResult BurialRecords()
        {
            return View();
        }

        public IActionResult ConfirmationOfDelete()
        {
            return View();
        }

        public IActionResult SupervisedAnalysis()
        {
            return View();
        }

        public IActionResult UnsupervisedAnalysis()
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
