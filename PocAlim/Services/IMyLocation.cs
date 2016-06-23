using System;
using PocAlim.Services;

namespace PocAlim
{
	public interface IMyLocation
	{
		GPSCoord GetPositionCoord();
	}
}

