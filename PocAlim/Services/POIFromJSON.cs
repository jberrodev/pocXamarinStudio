using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocAlim.Services
{
    //Classe qui sert à désérialiser le fichier JSON
    public class POIFromJSON : MvxViewModel
    {
        public List<POIJSON> data { get; set; }
    }

    public class POIJSON
    {
		public string siret { get; set; }
		public int regroupement { get; set; } //0 single activité, 1 supermarché, 2 multi activité
		public string nom { get; set; }
        public double lattitude { get; set; }
        public double longitude { get; set; }
        public string type { get; set; }
        public string adresse { get; set; }
		public IList<ActiviteJSON> activites { get; set; }

	}

	public class ActiviteJSON
	{
		public string nom { get; set; }
		public string note { get; set; }
		public string date { get; set; }
		
	}


}

