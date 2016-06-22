using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocAlim.Services
{
    public class MyFilter : IMyFilter
    {
		public String Reload(bool filtreCharcuteriesIsChecked,
		                     bool filtreBoucheriesIsChecked,
		                     bool filtrePoissonneriesIsChecked,
		                     bool filtreFromageriesIsChecked,
		                     bool filtreTraiteursIsChecked,
		                     bool filtreGlaciersIsChecked,
		                     bool filtreChocolatiersIsChecked,
		                     bool filtreBoulangeriesPatisseriesIsChecked,
		                     bool filtreAlimentationGeneraleIsChecked,
							 bool filtreSupermarchesHypermarchesIsChecked,
		                     bool filtreRestaurantsIsChecked,
							 bool filtreRestaurationCollectiveIsChecked)
        {
         
			String list = "";

			if (filtreCharcuteriesIsChecked)
				list += "Charcuteries,";
			if (filtreBoucheriesIsChecked)
				list += "Boucheries,";
			if (filtrePoissonneriesIsChecked)
				list += "Poissonneries,";
			if (filtreFromageriesIsChecked)
				list += "Fromageries,";
			if (filtreTraiteursIsChecked)
				list += "Traiteurs,";
			if (filtreGlaciersIsChecked)
				list += "Glaciers,";
			if (filtreChocolatiersIsChecked)
				list += "Chocolatiers,";
			if (filtreBoulangeriesPatisseriesIsChecked)
				list += "Boulangeries Patisseries,";
			if (filtreAlimentationGeneraleIsChecked)
				list += "Alimentation Generale,";
			if (filtreSupermarchesHypermarchesIsChecked)
				list += "Supermarches Hypermarches,";
			if (filtreRestaurantsIsChecked)
				list += "Restaurants,";
			if (filtreRestaurationCollectiveIsChecked)
				list += "Restauration Collective,";
	
		
            return list;

        }
    }
}
