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
using Android.Net;

namespace PocAlim.Droid.View
{

    /**Classe de création de la map
     * et ajout des markers**/
    [Activity(Label = "Map", Theme = "@style/MyTheme.NoTitle")]
	public class MyMapView : MvxActivity, IOnMapReadyCallback, Android.Gms.Maps.GoogleMap.IOnMyLocationButtonClickListener,IOnInfoWindowClickListener
    {

		public static readonly int InstallGooglePlayServicesId = 1000;
		private bool _isGooglePlayServicesInstalled;

        private GoogleMap _gMap;
		//marqueur temporaire pour la boucle d'ajout
        private Marker _marker;
		//Point cliqué par l'utilisateur
		private Marker _pointClick;

		//pour le gps checking
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

				//On vérifie la connexion internet
				bool test = isNetworkConnected();
				if (!test) {
					Toast.MakeText(this, "La connexion internet est necessaire", ToastLength.Short).Show();
				}


				if (_gMap == null) {
					FragmentManager.FindFragmentById<MapFragment> (Resource.Id.map).GetMapAsync (this);
				}
					
			}
			//sinon on cache notre view pour laisser la proposition
			//d'installation de google play services
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

			//Autorisation et positionnement du boutton zoom
            //_gMap.UiSettings.ZoomControlsEnabled = true;

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

			//infopopoupwindows custom
			//_gMap.SetInfoWindowAdapter(new CustomMarkerPopupAdapter(LayoutInflater));
			_gMap.SetOnInfoWindowClickListener(this);
        }

		//verification de l'état de la connexion
		private Boolean isNetworkConnected()
		{
			ConnectivityManager cm = (ConnectivityManager)GetSystemService(Context.ConnectivityService);

			return cm.ActiveNetworkInfo != null;
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
      
		//Clique map place un point
        private void MapOnMapClick(object sender, GoogleMap.MapClickEventArgs mapClickEventArgs)
        {
			if (_gMap != null)
			{
				if (_pointClick != null)
					_pointClick.Remove();
				
				MarkerOptions markerOpt1 = new MarkerOptions();
				markerOpt1.SetPosition(new LatLng(mapClickEventArgs.Point.Latitude,mapClickEventArgs.Point.Longitude));
				markerOpt1.SetTitle("Rechercher autour");
				_pointClick  = _gMap.AddMarker(markerOpt1);

				_pointClick.ShowInfoWindow();
			}

        }


		public void OnInfoWindowClick(Marker marker)
		{
			if (marker.Equals(_pointClick))
			Toast.MakeText(this, "fonction à développer ", ToastLength.Short).Show();

		}
	


        private void MapOnMarkerClick(object sender, GoogleMap.MarkerClickEventArgs markerClickEventArgs)
        {
            markerClickEventArgs.Handled = true;
            Marker marker = markerClickEventArgs.Marker;

            //zoom avec animation sur le marker
            // animateCameraOnMarker(marker);

			//zoom sans animation sur le marker
			_gMap.MoveCamera(CameraUpdateFactory.NewLatLng(marker.Position));

			//affichage des infos
			//marker.ShowInfoWindow();

			StartActivity(typeof(InfoPopupView));

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
			LatLng location = new LatLng(ViewModel.MyPositionCoord.Lat,ViewModel.MyPositionCoord.Lng);
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
			try{
				
			foreach (MyPOI marker in ViewModel.MarkerListFiltre)
                {
                    var option = new MarkerOptions();
                    option.SetPosition(new LatLng(marker.Coord.Lat, marker.Coord.Lng));
                    option.SetTitle(marker.Nom);

					switch(marker.Regroupement)
					{
						case 2:
							option.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Mipmap.marker_generique));
							break;
						case 1:
							option.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Mipmap.marker_supermarches_hypermarches));
							break;
						case 0:
							if (marker.Activites[0].NomActivite.Contains("Alimentation Generale"))
								option.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Mipmap.marker_alimentation_generale));
							else if (marker.Activites[0].NomActivite.Contains("Charcuteries"))
								option.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Mipmap.marker_charcuteries_boucheries));
							else if (marker.Activites[0].NomActivite.Contains("Boucheries"))
								option.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Mipmap.marker_charcuteries_boucheries));
							else if (marker.Activites[0].NomActivite.Contains("Restauration Collective"))
								option.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Mipmap.marker_restauration_collective));
							else if (marker.Activites[0].NomActivite.Contains("Restaurants"))
								option.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Mipmap.marker_restaurant));
							else if (marker.Activites[0].NomActivite.Contains("Boulangeries Patisseries"))
								option.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Mipmap.marker_boulangerie));
							else if (marker.Activites[0].NomActivite.Contains("Chocolatiers"))
								option.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Mipmap.marker_chocolatier));
							else if (marker.Activites[0].NomActivite.Contains("Glaciers"))
								option.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Mipmap.marker_glacier));
							else if (marker.Activites[0].NomActivite.Contains("Traiteurs"))
								option.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Mipmap.marker_traiteur));
							else if (marker.Activites[0].NomActivite.Contains("Fromageries"))
								option.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Mipmap.marker_fromagerie));
							else if (marker.Activites[0].NomActivite.Contains("Poissonneries"))
								option.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Mipmap.marker_poissonnerie));
							break;
							
						default:
							break;
					}


					if (_gMap != null) {
						_marker = _gMap.AddMarker (option);
					}
				}
			}
			catch (FormatException)
			{
				
			}catch (OverflowException)
			{
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

		public override void OnBackPressed()
		{
		}

		protected override void OnResume()
		{
			base.OnResume();

			//pour le cas où l'utilisateur revient sur l'appli
			//directement après la MAJ des google play services
			_isGooglePlayServicesInstalled = TestIfGooglePlayServicesIsInstalled();
			if (_isGooglePlayServicesInstalled)
			{
				FrameLayout myLayout = (FrameLayout)FindViewById(Resource.Id.myLayout);
				if (myLayout.Visibility == ViewStates.Invisible)
					myLayout.Visibility = ViewStates.Visible;
			}
		}

	}

}