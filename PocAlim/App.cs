using System;
using MvvmCross.Platform;
			using MvvmCross.Core.ViewModels;
using PocAlim.Services;
using PocAlim.ViewModels;

namespace PocAlim
{
	public class App : MvxApplication
	{
		public App ()
		{
			Mvx.RegisterType<IMyFilter, MyFilter>();
			Mvx.RegisterType<IMyLocation, MyLocation>();

			Mvx.RegisterSingleton<IMvxAppStart>(new MvxAppStart<FillingListOfMyPOIViewModel>());
		}

		}
}

