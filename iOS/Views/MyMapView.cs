using System;
using CoreLocation;
using MapKit;
using MvvmCross.iOS.Views;
using PocAlim.ViewModels;
using UIKit;

namespace PocAlim.iOS
{
	public partial class MyMapView : MvxViewController
	{
		public new FillingListOfMyPOIViewModel ViewModel
		{
			get { return (FillingListOfMyPOIViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var map = new MKMapView(UIScreen.MainScreen.Bounds);
			View = map;
				
			CLLocationManager locationManager = new CLLocationManager();
			locationManager.RequestWhenInUseAuthorization();

			map.ShowsUserLocation = true;

			// add an annotation
			map.AddAnnotations(new MKPointAnnotation()
			{
				Title = "MyAnnotation",
				Coordinate = new CLLocationCoordinate2D(ViewModel.MyPositionCoord.Lat,ViewModel.MyPositionCoord.Lng)
			});

		}

	}
}


