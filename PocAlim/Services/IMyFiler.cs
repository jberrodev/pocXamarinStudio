using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocAlim.Services
{
    public interface IMyFilter
    {
		String Reload(bool filtreRestaurantIsChecked, bool filtreProximiteIsChecked, bool filtreTransformationIsChecked, bool filtreSupermarcheIsChecked);
    }
}
