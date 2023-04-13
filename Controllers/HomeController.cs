using Egypt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Egypt.Data;
using Egypt.Models.ViewModels;
using Egypt.Models.Forms;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Tensorflow;
using Newtonsoft.Json;

namespace Egypt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private mummiesContext context { get; set; }
        private ApplicationDbContext identityContext { get; set; }

        public HomeController(ILogger<HomeController> logger, mummiesContext temp_context, ApplicationDbContext temp_id_context)
        {
            _logger = logger;
            context = temp_context;
            identityContext = temp_id_context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Burials(string table, int pageNum = 1)
        {
            ViewBag.IsAuthenticated = HttpContext.User.Identity.IsAuthenticated;
            ViewBag.SelectedTable = table;
            ViewBag.SelectedPage = pageNum;

            int pageSize = 50;
            int totalNumBurials;

            if (table == "BurialMain")
            {
                totalNumBurials = context.Burialmain.Count();
            }
            else if (table == "BodyAnalysisChart")
            {
				totalNumBurials = context.Bodyanalysischart.Count();
			}
			else if (table == "Textiles")
			{
				totalNumBurials = context.Textile.Count();
			}
			else if (table == "TextileStructures")
			{
				totalNumBurials = context.StructureTextile.Count();
			}
			else if (table == "TextileFunctions")
			{
				totalNumBurials = context.Textilefunction.Count();
			}
			else if (table == "TextileColors")
			{
				totalNumBurials = context.Color.Count();
			}
			else
            {
				totalNumBurials = context.Burialmain.Count();
			}


            var viewModel = new BurialsViewModel
            {
                Burials = context.Burialmain.OrderBy(x => x.Id).Skip((pageNum - 1) * pageSize).Take(pageSize),
                Bodyanalysischarts = context.Bodyanalysischart.OrderBy(x => x.Id).Skip((pageNum - 1) * pageSize).Take(pageSize),
                Textiles = context.Textile.OrderBy(x => x.Id).Skip((pageNum - 1) * pageSize).Take(pageSize),
                TextileStructures = context.Structure.OrderBy(x => x.Id).Skip((pageNum - 1) * pageSize).Take(pageSize),
                TextileColors = context.Color.OrderBy(x => x.Id).Skip((pageNum - 1) * pageSize).Take(pageSize),
                TextileFunctions = context.Textilefunction.OrderBy(x => x.Id).Skip((pageNum - 1) * pageSize).Take(pageSize),

                PageInfo = new PageInfo
                {
					TotalNumBurials = totalNumBurials,
					BurialsPerPage = pageSize,
                    CurrentPage = pageNum
                }
            };


			return View(viewModel);
        }

        [HttpPost]
        public IActionResult Burials(BurialMainFiltersForm bm, BodyAnalysisChartFiltersForm bac, TextileFiltersForm t, StructureFiltersForm s, FunctionFiltersForm f, ColorFiltersForm c, string table, int pageNum = 1)
        {
            ViewBag.IsAuthenticated = HttpContext.User.Identity.IsAuthenticated;
            ViewBag.SelectedTable = table;
            ViewBag.SelectedPage = pageNum;
            int pageSize = 50;

			IQueryable<dynamic> bigQuery = null;

            switch (table)
            {
                case "BodyAnalysisChart":
                    var bodyAnalysisQuery = context.Bodyanalysischart.AsQueryable();

                    if (bac.EstimateStature.HasValue) { bodyAnalysisQuery = bodyAnalysisQuery.Where(x => x.Estimatestature == bac.EstimateStature); }

                    bigQuery = bodyAnalysisQuery;

                    break;

                case "Textiles":
                    var textileQuery = context.Textile.AsQueryable();

                    if (!string.IsNullOrEmpty(t.Locale)) { textileQuery = textileQuery.Where(x => x.Locale == t.Locale); }

                    bigQuery = textileQuery;

                    break;

                case "TextileStructures":
                    var structureQuery = context.Structure.AsQueryable();

                    if (!string.IsNullOrEmpty(s.StructureValue)) { structureQuery = structureQuery.Where(x => x.Value == s.StructureValue); }

                    bigQuery = structureQuery;

                    break;

                case "TextileFunctions":
                    var functionQuery = context.Textilefunction.AsQueryable();

                    if (!string.IsNullOrEmpty(f.TextileFunction)) { functionQuery = functionQuery.Where(x => x.Value == f.TextileFunction); }

                    bigQuery = functionQuery;

                    break;

                case "TextileColors":
                    var colorsQuery = context.Color.AsQueryable();

                    if (!string.IsNullOrEmpty(c.TextileColor)) { colorsQuery = colorsQuery.Where(x => x.Value == c.TextileColor); }

                    bigQuery = colorsQuery;

                    break;


                default:

                    var burialQuery = context.Burialmain.AsQueryable();

                    if (!string.IsNullOrEmpty(bm.SquareNorthSouth)) { burialQuery = burialQuery.Where(x => x.Squarenorthsouth == bm.SquareNorthSouth); }

                    if (!string.IsNullOrEmpty(bm.NorthSouth)) { burialQuery = burialQuery.Where(x => x.Northsouth == bm.NorthSouth); }

                    if (!string.IsNullOrEmpty(bm.SquareEastWest)) { burialQuery = burialQuery.Where(x => x.Squareeastwest == bm.SquareEastWest); }

                    if (!string.IsNullOrEmpty(bm.EastWest)) { burialQuery = burialQuery.Where(x => x.Eastwest == bm.EastWest); }

                    if (!string.IsNullOrEmpty(bm.Area)) { burialQuery = burialQuery.Where(x => x.Area == bm.Area); }

                    if (bm.BurialNumber.HasValue) { burialQuery = burialQuery.Where(x => x.Burialnumber == bm.BurialNumber.Value.ToString()); }

                    if (!string.IsNullOrEmpty(bm.HeadDirection)) { burialQuery = burialQuery.Where(x => x.Headdirection == bm.HeadDirection); }

                    if (!string.IsNullOrEmpty(bm.AgeAtDeath)) { burialQuery = burialQuery.Where(x => x.Ageatdeath == bm.AgeAtDeath); }

                    if (!string.IsNullOrEmpty(bm.Sex)) { burialQuery = burialQuery.Where(x => x.Sex == bm.Sex); }

                    if (!string.IsNullOrEmpty(bm.HairColor)) { burialQuery = burialQuery.Where(x => x.Haircolor == bm.HairColor); }

                    bigQuery = burialQuery;

                    break;
            }



            var viewModel = new BurialsViewModel
            {
                Burials = bigQuery.OfType<Burialmain>().OrderBy(x => x.Id).Skip((pageNum - 1) * pageSize).Take(pageSize),
                Bodyanalysischarts = bigQuery.OfType<Bodyanalysischart>().OrderBy(x => x.Id),
                Textiles = bigQuery.OfType<Textile>().OrderBy(x => x.Id),
                TextileStructures = bigQuery.OfType<Structure>().OrderBy(x => x.Id),
                TextileColors = bigQuery.OfType<Color>().OrderBy(x => x.Id),
                TextileFunctions = bigQuery.OfType<Textilefunction>().OrderBy(x => x.Id),

				PageInfo = new PageInfo
				{
					TotalNumBurials = context.Burialmain.Count(),
					BurialsPerPage = pageSize,
					CurrentPage = 1
				}
			};

            return View("Burials", viewModel);
        }


        public IActionResult Users()
        {
            var usersinfo = identityContext.Users.OrderBy(x => x.Id).ToList();

            return View(usersinfo);
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

        [HttpGet]
        public IActionResult BurialDetails(long id)
        {
            Burialmain burialinfo = context.Burialmain.Where(b => b.Id == id).FirstOrDefault();
            return PartialView("_BurialDetailsPartial", burialinfo);
        }

        public IActionResult SupervisedAnalysis()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SupervisedAnalysis(InputData data)
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri("https://localhost:44317/predict");

                var json = JsonConvert.SerializeObject(data);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(uri, content);

                var result = await response.Content.ReadAsStringAsync();

                var prediction = JsonConvert.DeserializeObject<PredictionOutput>(result);

                ViewBag.Prediction = prediction;

                return View();

            }
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
