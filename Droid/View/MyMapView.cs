using System;

using Android.App;
using Android.Content;
using Android.Widget;
using MvvmCross.Droid.Views;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using static Android.Gms.Maps.GoogleMap;
using Android.Support.V4.Content;
using Android;
using Android.Content.PM;
using Android.Locations;
using PocAlim.ViewModels;
using PocAlim.Services;
using Android.Gms.Common;
using Android.Views;

namespace PocAlim.Droid.View
{

    /**Classe de création de la map
     * et ajout des markers**/
    [Activity(Label = "Map", Theme = "@style/MyTheme.NoTitle")]
    public class MyMapView : MvxActivity, IOnMapReadyCallback, Android.Gms.Maps.GoogleMap.IOnMyLocationButtonClickListener
    {

		public static readonly int InstallGooglePlayServicesId = 1000;
		private bool _isGooglePlayServicesInstalled;

        private GoogleMap _gMap;
        private Marker _marker;
        LocationManager _locationManager;


        //Specification du ViewModel
        public new FillingListOfMyPOIViewModel ViewModel
        {
            get { return (FillingListOfMyPOIViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
        //Une fois le ViewModel chargé on genere la vue
        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();

			//On vérifie si les Google Play Services sont dispo
			_isGooglePlayServicesInstalled = TestIfGooglePlayServicesIsInstalled();
			// Si OUI, on charge le Layout de manière classique
			if (_isGooglePlayServicesInstalled) {
				SetContentView (Resource.Layout.View_Map);

				if (_gMap == null)
					FragmentManager.FindFragmentById<MapFragment> (Resource.Id.map).GetMapAsync (this);
			}
			else
			{
				SetContentView(Resource.Layout.View_Map);
				if (_gMap == null)
					FragmentManager.FindFragmentById<MapFragment> (Resource.Id.map).GetMapAsync (this);
				FrameLayout myLayout = (FrameLayout)FindViewById(Resource.Id.myLayout);
				myLayout.Visibility = ViewStates.Invisible;
			}
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            _gMap = googleMap;
            _gMap.UiSettings.ZoomControlsEnabled = true;

            //Verification des permissions  de localisation
            checkLocationPermission();

            //Listener sur click d'un marker
            _gMap.MarkerClick += MapOnMarkerClick;

            //Listener sur click de la map
            _gMap.MapClick += MapOnMapClick;

            //Position de départ de la camera
            moveCameraStart();

            //parcours de la liste de markers du ViewModel
            //et ajout des markers à la map
            addMarkers();
            _gMap.SetInfoWindowAdapter(new CustomMarkerPopupAdapter(LayoutInflater));
        }

        //Verification de l'autorisation de localisation
        public void checkLocationPermission()
        {
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation)
                == (int)Permission.Granted)
            {
                //Affichage du Bouton de localisation google
                _gMap.MyLocationEnabled = true;
                _gMap.SetOnMyLocationButtonClickListener(this);
            }
            else
            {
                Toast.MakeText(this, "Location Permissions are required !", ToastLength.Short).Show();
            }

        }

        //Listener du bouton de localisation google
        bool Android.Gms.Maps.GoogleMap.IOnMyLocationButtonClickListener.OnMyLocationButtonClick()
        {
            bool _isGpsEnable = false;
            _isGpsEnable=checkGPS();

            if (_isGpsEnable)
            {
                //le gps est activé
                //return false zoom sur la localisation
                return false;
            }
            else
            {
                Toast.MakeText(this, String.Format("Veuillez Activer le GPS"), ToastLength.Short).Show();
                return true;
            }
        }

        //vérification de l'activation du GPS
        public bool checkGPS()
        {
            _locationManager = GetSystemService(Context.LocationService) as LocationManager;

            string provider = LocationManager.GpsProvider;

            if (_locationManager.IsProviderEnabled(provider))
            {
                return true;
            }
            return false;
        }
      

        private void MapOnMapClick(object sender, GoogleMap.MapClickEventArgs mapClickEventArgs)
        {
           // Toast.MakeText(this, String.Format("You clicked on the MAP"), ToastLength.Short).Show();

        }

        private void MapOnMarkerClick(object sender, GoogleMap.MarkerClickEventArgs markerClickEventArgs)
        {
            markerClickEventArgs.Handled = true;
            Marker marker = markerClickEventArgs.Marker;

            //zoom avec animation sur le marker
            //cliqué avec un décallage pour
            //laisser de la place à la infowindow
             animateCameraOnMarker(marker);
            
            //affichage des infos
            marker.ShowInfoWindow();
        }

        //zoom avec animation sur le marker
        //cliqué avec un décallage pour
        //laisser de la place à la infowindow
        public void animateCameraOnMarker(Marker marker)
        {
            double _latToZoom = marker.Position.Latitude;
            double _lngToZoom = marker.Position.Longitude;

            _gMap.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(_latToZoom,_lngToZoom), _gMap.CameraPosition.Zoom));
        }

        //Position de départ de la camera
        public void moveCameraStart()
        {
                LatLng location = new LatLng(48.828808,2.261146);
                CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                builder.Target(location);
                builder.Zoom(14);
                CameraPosition cameraPosition = builder.Build();
                CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            if (_gMap != null)
            {
                _gMap.MoveCamera(cameraUpdate);
            }
           
        }
        //parcours de la liste de markers du ViewModel
        //et ajout des markers à la map
        public void addMarkers()
        {
                foreach (MyPOI marker in ViewModel.MarkerListFiltre)
                {
                    var option = new MarkerOptions();
                    option.SetPosition(new LatLng(marker.Coord.Lat, marker.Coord.Lng));
                    option.SetTitle(marker.Nom);
                    if (marker.Type.Contains("Restaurant"))
                        option.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin_restaurant));
                    if (marker.Type.Contains("Proximite"))
                        option.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin_proximite));
                    if (marker.Type.Contains("Supermarche"))
                        option.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin_supermarche));
                    if (marker.Type.Contains("Transformation"))
                        option.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin_transformation));

                    if (_gMap != null)
                        _marker = _gMap.AddMarker(option);
                }
        }

		private bool TestIfGooglePlayServicesIsInstalled()
		{
			int queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
			if (queryResult == ConnectionResult.Success)
			{
				return true;
			}
			return false;
		}

       
    }

}