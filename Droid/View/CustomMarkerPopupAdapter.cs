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
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using MvvmCross.Droid.Views;
using Android.Support.V4.App;
using Android.Support.V4.View;

namespace PocAlim.Droid.View
{
	public class CustomMarkerPopupAdapter : Java.Lang.Object, GoogleMap.IInfoWindowAdapter
    {
        private LayoutInflater _layoutInflater = null;
        
        public CustomMarkerPopupAdapter(LayoutInflater inflater)
        {
            //This constructor does hit a breakpoint and executes
            _layoutInflater = inflater;

		}

        public Android.Views.View GetInfoContents(Marker marker)
        {
            //This never executes or hits a break point
            var customPopup = _layoutInflater.Inflate(Resource.Layout.View_Info_Window, null);

            var titleTextView = customPopup.FindViewById<TextView>(Resource.Id.custom_marker_popup_title);
            if (titleTextView != null)
            {
                titleTextView.Text = marker.Title;
            }
            return customPopup;
        }

        public Android.Views.View GetInfoWindow(Marker marker)
        {
            var customPopup = _layoutInflater.Inflate(Resource.Layout.View_Info_Window, null);

            var titleTextView = customPopup.FindViewById<TextView>(Resource.Id.custom_marker_popup_title);
            if (titleTextView != null)
            {
                titleTextView.Text = marker.Title;
            }
            return customPopup;
        }
	}


}