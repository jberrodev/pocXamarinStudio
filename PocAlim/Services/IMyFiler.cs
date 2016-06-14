using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocAlim.Services
{
    public interface IMyFilter
    {
        List<string> Reload(bool filtreRestaurantIsChecked, bool filtreProximiteIsChecked, bool filtreTransformationIsChecked, bool filtreSupermarcheIsChecked);
    }
}
