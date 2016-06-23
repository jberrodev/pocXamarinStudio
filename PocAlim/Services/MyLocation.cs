using System;
using PocAlim.Services;

namespace PocAlim
{
	public class MyLocation : IMyLocation
	{
		public GPSCoord GetPositionCoord()
		{
			GPSCoord myPositionCoord = new GPSCoord() { Lat = 48.828808, Lng = 2.261146 };

			return myPositionCoord;
		}
	}
}

