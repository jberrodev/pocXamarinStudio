using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PocAlim.Services;
using MvvmCross.Core.ViewModels;

namespace PocAlim.ViewModels
{
    public class FillingListOfMyPOIViewModel : MvxViewModel
    {
        private List<MyPOI> _markerslist;

        public List<MyPOI> MarkerList
        {
            get { return _markerslist; }
            set { _markerslist = value; RaisePropertyChanged(() => MarkerList); }
        }

        string jsonString2 = @"
    {
        ""data"": [
            {
                ""nom"": ""Sogeti France"",
                ""lattitude"": ""48.826870"",
                ""longitude"": ""2.271165"",
                ""type"": ""Restaurant"",
                ""adresse"": ""22 rue Gouverneur General Eboue, 92130 Issy Les Moulineaux"",
                ""note"": ""5"",
                ""inspection"": ""01/01/2015""
            },
            {
                 ""nom"": ""Quelque part"",
                ""lattitude"": ""48.831772"",
                ""longitude"": ""2.262446"",
                ""type"": ""Proximite"",
                ""adresse"": ""18, Rue du Test, 92100 Boulogne-Billancourt"",
                ""note"": ""2"",
                ""inspection"": ""01/01/2016""
            },
            {
                 ""nom"": ""Quelque part ailleurs"",
                ""lattitude"": ""48.831165"",
                ""longitude"": ""2.254237"",
                ""type"": ""Supermarche"",
                ""adresse"": ""18,rue d'ailleurs, 92100 Boulogne-Billancourt"",
                ""note"": ""1"",
                ""inspection"": ""01/01/2016""
            },
            {
                 ""nom"": ""Quelque part d'autre"",
                ""lattitude"": ""48.828851"",
                ""longitude"": ""2.266948"",
                ""type"": ""Transformation"",
                ""adresse"": ""123 Avenue d'autre part, 92130 Issy Les Moulineaux"",
                ""note"": ""3"",
                ""inspection"": ""01/01/2016""
            },
  			{
                 ""nom"": ""Hello mon ami"",
                ""lattitude"": ""48.826551"",
                ""longitude"": ""2.257548"",
                ""type"": ""Transformation"",
                ""adresse"": ""t'aime ca manger des papates"",
                ""note"": ""1"",
                ""inspection"": ""01/01/2016""
            },
			{
                 ""nom"": ""Dr pepper"",
                ""lattitude"": ""48.822913"",
                ""longitude"": ""2.260731"",
                ""type"": ""Transformation"",
                ""adresse"": ""moi non"",
                ""note"": ""1"",
                ""inspection"": ""01/01/2016""
            }

        ]
    }";

        public override void Start()
        {
            _markerslist = new List<MyPOI>();
            //On parcours le résultat en remplissant la liste

            //de Markers qui sera utilisée par les couches natives
            loadJson();

            base.Start();
        }

        //On parcours le résultat en remplissant la liste
        //de Markers qui sera utilisée par les couches natives
        public void loadJson()
        {
          
                var des = (POIFromJSON)Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString2, typeof(POIFromJSON));

                foreach (POIJSON markerJson in des.data)
                {
                    try
                    {
                    var marker = new MyPOI()
                    {
                        Coord = new GPSCoord() { Lat = Convert.ToDouble(markerJson.lattitude), Lng = Convert.ToDouble(markerJson.longitude) },
                        Nom = markerJson.nom,
                        Type = markerJson.type,
                        Adresse = markerJson.adresse,
                        Note = Convert.ToDecimal(markerJson.note),
                        Inspection = markerJson.inspection

                        };
                        _markerslist.Add(marker);
                    }
                    catch (FormatException)
                    {
                    }
                    catch (OverflowException)
                    {
                    }

                }
            }

        public ICommand GoPopupFiltre
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<FilterViewModel>());
            }
        }

		private String[] _filtre;

		public String[] Filtre
		{
			get { return _filtre; }
			set { _filtre = value; RaisePropertyChanged(() => Filtre); }
		}

		private string _temoin;

		public String Temoin
		{
			get { return _temoin; }
			set { _temoin = value; RaisePropertyChanged(() => Temoin); }
		}

		public void Init(string filtreToPass)
		{
			_temoin = filtreToPass;
		}

        
    }
}


