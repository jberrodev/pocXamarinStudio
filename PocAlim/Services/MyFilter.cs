using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocAlim.Services
{
    public class MyFilter : IMyFilter
    {
		public String Reload(bool filtreRestaurationCollectiveIsChecked, bool filtreAlimentationGeneraleIsChecked, bool filtreCharcuteriesIsChecked, bool filtreSupermarchesHypermarchesIsChecked)
        {
         
			String list = "";

			if (filtreRestaurationCollectiveIsChecked)
				list += "Restauration Collective,";
			if (filtreAlimentationGeneraleIsChecked)
				list +="Alimentation Generale,";
			if (filtreCharcuteriesIsChecked)
				list+="Charcuteries,";
			if (filtreSupermarchesHypermarchesIsChecked)
				list+="Supermarches Hypermarches,";

            return list;

        }
    }
}
