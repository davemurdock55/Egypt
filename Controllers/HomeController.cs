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
            return View();
        }

        public IActionResult Burials()
        {
            var burialinfo = context.Burialmain.OrderBy(x => x.Id).ToList();

            return View(burialinfo);
        }

        public IActionResult Privacy()
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
