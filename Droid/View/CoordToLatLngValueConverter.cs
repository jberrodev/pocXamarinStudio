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
using MvvmCross.Platform.Converters;
using Android.Gms.Maps.Model;
using System.Globalization;
using PocAlim.Services;

namespace PocAlim.Droid.View
{
    //Convertisseur 
    //de la classe maison 'GPSCoord'
    //a la classe Google 'LatLng'
    public class CoordToLatLngValueConverter : MvxValueConverter<GPSCoord, LatLng>
    {
        protected override LatLng Convert(GPSCoord value, Type targetType, object parameter, CultureInfo culture)
        {
            return new LatLng(value.Lat, value.Lng);
        }
        protected override GPSCoord ConvertBack(LatLng value, Type targetType, object parameter, CultureInfo culture)
        {
            return new GPSCoord() { Lat = value.Latitude, Lng = value.Longitude };
        }
    }
}