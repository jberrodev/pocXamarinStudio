using System;

using Android.App;
using Android.Content;
using Android.Widget;
using MvvmCross.Droid.Views;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Location;
using Android.Gms.Common.Apis;
using Android.Locations;
using static Android.Gms.Maps.GoogleMap;
using Android.Support.V4.Content;
using Android;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Views;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Util;

using PocAlim.ViewModels;
using PocAlim.Services;
using System.Threading.Tasks;

namespace PocAlim.Droid.View
{

    /**Classe de création de la map
     * et ajout des markers**/
    [Activity(Label = "Map", Theme = "@style/MyTheme.NoTitle")]
	public class MyMapView : MvxActivity, IOnMapReadyCallback ,IOnInfoWindowClickListener, GoogleApiClient.IConnectionCallbacks,
		GoogleApiClient.IOnConnectionFailedListener
    {

		public static readonly int InstallGooglePlayServicesId = 1000;
		private bool _isGooglePlayServicesInstalled;

		private ImageButton moveToMyLocationButton;
		public bool needToSetCamera = true;

		private GoogleApiClient client;
		protected LocationRequest request;
		private LocationSettingsRequest settingsRequest;
        private GoogleMap _gMap;

		//marqueur temporaire pour la boucle d'ajout
        private Marker _marker;
		//Point cliqué par l'utilisateur
		private Marker _pointClick;

		//pour le gps checking
        LocationManager _locationManager;

        //private LocationManager _locationManager;
		private double lat = 0;
		private double lng = 0;

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

				if (_gMap == null) {
					FragmentManager.FindFragmentById<MapFragment> (Resource.Id.map).GetMapAsync (this);
				}

				BuildGoogleApiClient();
				CreateLocationRequest();
				BuildLocationSettingsRequest();

			}
			//sinon on cache notre view pour laisser la proposition
			//d'installation de google play services
			else
			{
				SetContentView(Resource.Layout.View_Map);
				if (_gMap == null)
					FragmentManager.FindFragmentById<MapFragment> (Resource.Id.map).GetMapAsync (this);
				RelativeLayout myLayout = (RelativeLayout)FindViewById(Resource.Id.myLayout);
				myLayout.Visibility = ViewStates.Invisible;
			}
        }

		public void OnMapReady(GoogleMap googleMap)
		{
			_gMap = googleMap;
			_gMap.UiSettings.CompassEnabled = false;

			//Listener sur click d'un marker
			_gMap.MarkerClick += MapOnMarkerClick;

			//Listener sur click de la map
			_gMap.MapClick += MapOnMapClick;

			startGeoloc();

			//si la connection internet n'est pas activée
			if (!isNetworkConnected())
			{
				Toast.MakeText(this, "Une connexion internet est necessaire pour charger les etablissements", ToastLength.Short).Show();
			}
			//si la connection internet n'est pas activée
			if (isNetworkConnected())
			{
				//parcours de la liste de markers du ViewModel
				//et ajout des markers à la map
				addMarkers();
			}

			//infopopoupwindows custom
			_gMap.SetInfoWindowAdapter(new CustomMarkerPopupAdapter(LayoutInflater));
			_gMap.SetOnInfoWindowClickListener(this);

        }

		//verification de l'état de la connexion
		private Boolean isNetworkConnected()
		{
			ConnectivityManager cm = (ConnectivityManager)GetSystemService(Context.ConnectivityService);

			return cm.ActiveNetworkInfo != null;
		}

		//Verification de l'autorisation de localisation
		public bool isLocationPermission()
		{
			if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == (int)Permission.Granted)
			{
				return true;
			}
			else
			{
				//Toast.MakeText(this, "Location Permissions are required !", ToastLength.Long).Show();
				return false;
			}
		}

		//Vérifie que le GPS est activé
		public bool isGPSActivate() 
		{ 
			LocationManager locMgr = GetSystemService(Context.LocationService) as LocationManager;
			string Provider = LocationManager.GpsProvider;

			if (locMgr.IsProviderEnabled(Provider)) { return true; }
			else { return false; }
		}
      
		public void MoveToMyLocation(object sender, EventArgs e)
		{
			moveCamera();
		}

		public async void openGPS(object sender, EventArgs e) 
		{
			await CheckLocationSettings();
		}

		protected void BuildGoogleApiClient()
		{
			Log.Info("Client", "Building GoogleApiClient");
			client = new GoogleApiClient.Builder(this)
				.AddConnectionCallbacks(this)
				.AddOnConnectionFailedListener(this)
				.AddApi(LocationServices.API)
				.Build();
		}

		protected void CreateLocationRequest()
		{
			request = new LocationRequest();
			request.SetInterval(10000);
			request.SetFastestInterval(10000/2);
			request.SetPriority(LocationRequest.PriorityHighAccuracy);
		}

		protected void BuildLocationSettingsRequest()
		{
			LocationSettingsRequest.Builder builder = new LocationSettingsRequest.Builder();
			builder.AddLocationRequest(request);
			settingsRequest = builder.Build();
		}

		protected async Task CheckLocationSettings()
		{
			var result = await LocationServices.SettingsApi.CheckLocationSettingsAsync(client, settingsRequest);
			await HandleResult(result);
		}

		public async Task HandleResult(LocationSettingsResult locationSettingsResult)
		{
			var status = locationSettingsResult.Status;
			switch (status.StatusCode)
			{
				case CommonStatusCodes.ResolutionRequired:
					Log.Info("Client", "Location settings are not satisfied. Show the user a dialog to" +
					"upgrade location settings ");

					try
					{
						status.StartResolutionForResult(this, 0x1);
					}
					catch (IntentSender.SendIntentException)
					{
						Log.Info("Client", "PendingIntent unable to execute request.");
					}
					break;
				case LocationSettingsStatusCodes.SettingsChangeUnavailable:
					Log.Info("Client", "Location settings are inadequate, and cannot be fixed here. Dialog " +
					"not created.");
					break;
			}
		}

		protected override async void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			switch (requestCode)
			{
				case 0x1:
					switch (resultCode)
					{
						case Result.Ok:
							Log.Info("Client", "User agreed to make required location settings changes.");
							startGeoloc();
							break;
						case Result.Canceled:
							Log.Info("Client", "User chose not to make required location settings changes.");
							break;
					}
					break;
			}
		}

		public void startGeoloc() 
		{
			//si on a les permissions de localisation et le gps activé
			if (isLocationPermission() && isGPSActivate())
			{
				//Affichage de la position de l'utilisateur
				_gMap.MyLocationEnabled = true;
				_gMap.UiSettings.MyLocationButtonEnabled = false;

				//Listener sur les chagements de localisation
				_gMap.MyLocationChange += (object sender, MyLocationChangeEventArgs e) =>
				{
					lat = e.Location.Latitude;
					lng = e.Location.Longitude;

					//Position de départ de la camera
					if (needToSetCamera)
					{
						moveCamera();
						needToSetCamera = false;
					}
				};

				moveToMyLocationButton.SetImageResource(Resource.Mipmap.group_5);
				//Listener sur le bouton de localisation
				moveToMyLocationButton.Click += MoveToMyLocation;
			}

			//si on a pas les permissions de localisation ou le gps désactivé
			if (!isLocationPermission() || !isGPSActivate())
			{
				//On change l'icone du bouton de géoloc
				moveToMyLocationButton.SetImageResource(Resource.Mipmap.group_7);

				moveToMyLocationButton.Click += openGPS;
				Toast.MakeText(this, "GPS is not available. Does the device have location services enabled?", ToastLength.Long).Show();
			}
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
				markerOpt1.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Mipmap.pin_restauration));
				_pointClick  = _gMap.AddMarker(markerOpt1);
				_gMap.AnimateCamera(CameraUpdateFactory.NewLatLng(new LatLng(mapClickEventArgs.Point.Latitude, mapClickEventArgs.Point.Longitude)));

				_pointClick.ShowInfoWindow();
			}

        }


		public void OnInfoWindowClick(Marker marker)
		{
			if (marker.Equals(_pointClick))
			Toast.MakeText(this, "fonction a developper ", ToastLength.Short).Show();

		}

        private void MapOnMarkerClick(object sender, GoogleMap.MarkerClickEventArgs markerClickEventArgs)
        {
            markerClickEventArgs.Handled = true;
            Marker marker = markerClickEventArgs.Marker;

			//zoom avec animation sur le marker
			// animateCameraOnMarker(marker);

			//zoom sans animation sur le marker
			//_gMap.MoveCamera(CameraUpdateFactory.NewLatLng(marker.Position));

			//affichage des infos
			//marker.ShowInfoWindow();
			if (!marker.Equals(_pointClick)) 
			{ 
				_pointClick.ShowInfoWindow();
				//StartActivity(typeof(InfoPopupView));
			}
			else { _pointClick.Remove(); }

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

        //Positionnne la camera
        public void moveCamera()
        {
			LatLng location = new LatLng(lat,lng);
                CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                builder.Target(location);
                builder.Zoom(14);
                CameraPosition cameraPosition = builder.Build();
                CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            if (_gMap != null)
            {
                _gMap.AnimateCamera(cameraUpdate);
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

		//Gestion du bouton back
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
				RelativeLayout myLayout = (RelativeLayout)FindViewById(Resource.Id.myLayout);
				if (myLayout.Visibility == ViewStates.Invisible)
					myLayout.Visibility = ViewStates.Visible;
			}

			moveToMyLocationButton = (ImageButton)FindViewById(Resource.Id.locateBtn);
		}

		protected override void OnStart()
		{
			base.OnStart();
			client.Connect();
		}

		protected override void OnPause()
		{
			base.OnPause();
		}

		protected override void OnStop()
		{
			
			base.OnStop();
			client.Disconnect();
		}


		//Méthodes de l'interface GoogleApiClient
		public void OnConnected(Bundle connectionHint)
		{
			Log.Info("Client", "Connected to GoogleApiClient");
		}

		public void OnConnectionSuspended(int cause)
		{
			Log.Info("Client", "Connection suspended");
		}

		public void OnConnectionFailed(ConnectionResult result)
		{
			Log.Info("Client", "Connection failed: ConnectionResult.getErrorCode() = " + result.ErrorCode);
		}
	}

}