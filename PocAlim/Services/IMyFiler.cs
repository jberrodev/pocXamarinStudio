using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocAlim.Services
{
    public interface IMyFilter
    {
		String Reload(bool filtreBoucheriesCharcuteriesIsChecked,
							 bool filtrePoissonneriesIsChecked,
							 bool filtreFromageriesIsChecked,
							 bool filtreTraiteursIsChecked,
							 bool filtreGlaciersIsChecked,
							 bool filtreChocolatiersIsChecked,
							 bool filtreBoulangeriesPatisseriesIsChecked,
							 bool filtreAlimentationGeneraleIsChecked,
							 bool filtreSupermarchesHypermarchesIsChecked,
							 bool filtreRestaurantsIsChecked,
							 bool filtreRestaurationCollectiveIsChecked);
    }
}
