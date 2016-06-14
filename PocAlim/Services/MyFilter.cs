using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocAlim.Services
{
    public class MyFilter : IMyFilter
    {
        public List<string> Reload(bool filtreRestaurantIsChecked, bool filtreProximiteIsChecked, bool filtreTransformationIsChecked, bool filtreSupermarcheIsChecked)
        {
         
            List<string> list = new List<string>();
            if (filtreRestaurantIsChecked)
                list.Add("Restaurant");
            if (filtreProximiteIsChecked)
                list.Add("Proximité");
            if (filtreTransformationIsChecked)
                list.Add("Transformation");
            if (filtreSupermarcheIsChecked)
                list.Add("Supermarché");

            return list;

        }
    }
}
