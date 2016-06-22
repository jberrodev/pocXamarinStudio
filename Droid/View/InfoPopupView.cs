using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using PocAlim.Droid.View;

namespace PocAlim.Droid.View
{
	[Activity(Theme = "@style/MyTheme.Popup")]
	public class FragmentTest : FragmentActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.View_Info_Popup);

			var _carrousselViewPager = FindViewById<ViewPager>(Resource.Id.myCaroussel);
			_carrousselViewPager.Adapter = new CarousselFragmentAdapter(SupportFragmentManager);

		}
	}

	public class CarousselFragmentAdapter : FragmentPagerAdapter
	{
		public CarousselFragmentAdapter(Android.Support.V4.App.FragmentManager fm) : base(fm)
		{

		}

		public override int Count
		{
			get
			{
				return 3;
			}
		}

		public override Android.Support.V4.App.Fragment GetItem(int position)
		{
			return new CarrousselFragment();
		}
	}

	public class CarrousselFragment : Android.Support.V4.App.Fragment
	{
		public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{

			var view = inflater.Inflate(Resource.Layout.View_Caroussel, container, false);

			return view;
		}
	}
}