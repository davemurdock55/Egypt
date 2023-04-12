using System;
using System.Linq;
using Egypt.Models;
using Microsoft.AspNetCore.Mvc;

namespace Egypt.Components
{
	public class FiltersViewComponents : ViewComponent
	{
		private mummiesContext context { get; set; }


		public FiltersViewComponents(mummiesContext temp_context)
		{
			context = temp_context;
		}

		public IViewComponentResult Invoke()
		{
            ViewBag.SelectedFilter = RouteData?.Values["filter"];

			// we need to have some sort of an if statement, and depending on what table we have, we need different filters like Sex or Depth for Burialmain
			// or 

			var filters = context.Burialmain
				.Select(x => x.Area)
				.Distinct()
				.OrderBy(x => x);

			return View(filters);

			// sex, age at death, head direction, and burial id(combintation) are all in Burialmain
			// Estimate stature is in bodyanalysischart
			// all the textile ones are tables
		}
	}
}

