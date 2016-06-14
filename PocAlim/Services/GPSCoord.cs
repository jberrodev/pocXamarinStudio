using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocAlim.Services
{
    public class GPSCoord : MvxViewModel
    {
        private double _lat;
        private double _lng;

        public GPSCoord()
        {
            _lat = Lat;
            _lng = Lng;
        }

        public double Lat
        {
            get { return _lat; }
            set { _lat = value; RaisePropertyChanged(() => Lat); }
        }

        public double Lng
        {
            get { return _lng; }
            set { _lng = value; RaisePropertyChanged(() => Lng); }
        }
    }
}
