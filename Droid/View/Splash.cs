using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using System.Threading.Tasks;
using MvvmCross.Droid.Views;

namespace PocAlim.Droid
{
	[Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true, Icon="@drawable/splash_logo")]
    public class Splash : MvxSplashScreenActivity
    {
<<<<<<< HEAD
      public Splash()
		{

		}
=======
		public Splash() : base(Resource.Layout.layout_splashscreen) { }
>>>>>>> a29012c... commit de Aurel
    }

}
