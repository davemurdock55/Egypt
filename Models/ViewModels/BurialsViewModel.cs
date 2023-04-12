using System;
using System.Linq;

namespace Egypt.Models.ViewModels
{
    public class BurialsViewModel
    {
        public IQueryable<Burialmain> Burials { get; set; }

        public IQueryable<Bodyanalysischart> Bodyanalysischarts { get; set; }

        public IQueryable<Textile> Textiles { get; set; }

        public IQueryable<Structure> TextileStructures { get; set; }

        public IQueryable<Color> TextileColors { get; set; }

        public IQueryable<Textilefunction> TextileFunctions { get; set; }

        public PageInfo PageInfo { get; set; }

    }
}

