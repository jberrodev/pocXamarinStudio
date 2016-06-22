using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocAlim.Services
{
    //Objet Markers qui sera lu par les couches natives
   public class MyPOI : MvxViewModel
    {
        private GPSCoord _coord;
		private String _siret;
		private int _regroupement; //0 single activité, 1 supermarché, 2 multi activité
        private String _nom;
        private String _adresse;
		public IList<MyPOIActivite> _activites;


		public MyPOI()
        {
            _coord = Coord;
			_siret = Siret;
            _nom = Nom;
            _adresse = Adresse;
			_activites = Activites;
        }

        public GPSCoord Coord
        {
            get { return _coord; }
            set { _coord = value; RaisePropertyChanged(() => Coord); }
        }
		public String Siret
		{
			get { return _siret; }
			set { _siret = value; RaisePropertyChanged(() => Siret); }
		}
		public int Regroupement
		{
			get { return _regroupement; }
			set { _regroupement = value; RaisePropertyChanged(() => Regroupement); }
		}
        public String Nom
		{
			get { return _nom; }
			set { _nom = value; RaisePropertyChanged(() => Nom); }
		}
        public String Adresse
		{
			get { return _adresse; }
			set { _adresse = value; RaisePropertyChanged(() => Adresse); }
		}
		public IList<MyPOIActivite> Activites
		{
			get { return _activites; }
			set { _activites = value; RaisePropertyChanged(() => Activites); }
		}
    }

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

	public class MyPOIActivite : MvxViewModel
	{
		private string _nomActivite;
		private string _noteActivite;
		private string _dateActivite;

		public MyPOIActivite()
		{
			_nomActivite = NomActivite;
			_noteActivite = NoteActivite;
			_dateActivite = DateActivite;
		}

		public String NomActivite
		{
			get { return _nomActivite; }
			set { _nomActivite = value; RaisePropertyChanged(() => NomActivite); }
		}

		public String NoteActivite
		{
			get { return _noteActivite; }
			set { _noteActivite = value; RaisePropertyChanged(() => NoteActivite); }
		}

		public String DateActivite
		{
			get { return _dateActivite; }
			set { _dateActivite = value; RaisePropertyChanged(() => DateActivite); }
		}
	}
}
