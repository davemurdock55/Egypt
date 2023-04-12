using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Egypt.Models.Forms
{
    public class BurialMainFiltersForm
    {
        public string SquareNorthSouth { get; set; }
        public string NorthSouth { get; set; }
        public string SquareEastWest { get; set; }
        public string EastWest { get; set; }
        public string Area { get; set; }
        public int? BurialNumber { get; set; }
        public string HeadDirection { get; set; }
        public string AgeAtDeath { get; set; }
        public string Sex { get; set; }
        public string HairColor { get; set; }
    }
 
}

