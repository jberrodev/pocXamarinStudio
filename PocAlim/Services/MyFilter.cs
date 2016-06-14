using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocAlim.Services
{
    public class MyFilter : IMyFilter
    {
		public String Reload(bool filtreRestaurantIsChecked, bool filtreProximiteIsChecked, bool filtreTransformationIsChecked, bool filtreSupermarcheIsChecked)
        {
         
			String list = "";

            if (filtreRestaurantIsChecked)
				list += "Restaurant,";
            if (filtreProximiteIsChecked)
				list +="Proximité,";
            if (filtreTransformationIsChecked)
				list+="Transformation,";
            if (filtreSupermarcheIsChecked)
				list+="Supermarché,";

            return list;

        }
    }
}
