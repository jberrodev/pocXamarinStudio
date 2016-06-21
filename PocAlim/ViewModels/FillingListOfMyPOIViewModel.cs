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
using MvvmCross.Platform.Platform;

namespace PocAlim.ViewModels
{
    public class FillingListOfMyPOIViewModel : MvxViewModel
	{

		//Liste chargée depuis le Json
		private List<MyPOI> _markerslist;

        public List<MyPOI> MarkerList
        {
            get { return _markerslist; }
            set { _markerslist = value; RaisePropertyChanged(() => MarkerList); }
        }

		//Liste filtrée et utilisée par les view
		private List<MyPOI> _markersListFiltre;

		public List<MyPOI> MarkerListFiltre
		{
			get { return _markersListFiltre; }
			set { _markersListFiltre = value; RaisePropertyChanged(() => MarkerListFiltre); }
		}

		//Valeur des filtres reçus FilterViewModel
		private string[] _filtre;

		public String[] Filtre
		{
			get { return _filtre; }
			set { _filtre = value; RaisePropertyChanged(() => Filtre); }
		}

		//Temoin d'affichage #debugging
		private string _temoin;

		public String Temoin
		{
			get { return _temoin; }
			set { _temoin = value; RaisePropertyChanged(() => Temoin); }
		}

		//Liste de POi à remplacer par un Json local 
        string jsonString2 = @"
    {
        ""data"": [
            {
                ""nom"": ""Sogeti France"",
                ""lattitude"": 48.826870,
                ""longitude"": 2.271165,
                ""type"": ""Restauration Collective"",
                ""adresse"": ""22 rue Gouverneur General Eboue, 92130 Issy Les Moulineaux"",
				""activites"":
							[
								{ ""nom"": ""Restauration Collective "", ""note"": ""Satisfaisante"", ""date"": ""01/01/2015"" }
							],
            },
            {
                ""nom"": ""Quelque part"",
                ""lattitude"": 48.831772,
                ""longitude"": 2.262446,
                ""type"": ""Alimentation Generale"",
                ""adresse"": ""18, Rue du Test, 92100 Boulogne-Billancourt""
            },
            {
                ""nom"": ""Quelque part ailleurs"",
                ""lattitude"": 48.831165,
                ""longitude"": 2.254237,
                ""type"": ""Supermarches Hypermarches"",
                ""adresse"": ""18,rue ailleurs, 92100 Boulogne-Billancourt""
            },
            {
                ""nom"": ""Quelque part dautre"",
                ""lattitude"": 48.828851,
                ""longitude"": 2.266948,
                ""type"": ""Charcuteries"",
                ""adresse"": ""123 Avenue dautre part, 92130 Issy Les Moulineaux""
            },
  			{
                ""nom"": ""Hello mon ami"",
                ""lattitude"": 48.826551,
                ""longitude"": 2.257548,
                ""type"": ""Charcuteries"",
                ""adresse"": ""taime ca manger des papates""
			},
			{
                ""nom"": ""Dr pepper"",
                ""lattitude"": 48.822913,
                ""longitude"": 2.260731,
                ""type"": ""Charcuteries"",
                ""adresse"": ""moi non""
            },
			{
                ""nom"": ""Issy"",
                ""lattitude"": 48.820138,
                ""longitude"": 2.255601,
                ""type"": ""Restauration Collective, Supermarches Hypermarches"",
                ""adresse"": ""moi non""
            },
			{
                ""nom"": ""Seine"",
                ""lattitude"": 48.838601,
                ""longitude"": 2.269233,
                ""type"": ""Alimentation Generale"",
                ""adresse"": ""moi non""
            }

        ]
    }";
		//Au lancement du modelView...
        public override void Start()
        {
            _markerslist = new List<MyPOI>();
			_markersListFiltre = new List<MyPOI>();

            //On parcours le résultat en remplissant la liste
            //de Markers qui sera utilisée par les couches natives
            loadJson();

			//On définit MarkerListFiltre en fonction des filtres
			if (Filtre != null) {
				checkFilters();
			}
			//Sans filtre elle prend la valeur de MarkerList
			 else {
				_markersListFiltre = MarkerList;
			}
			 
			base.Start();
        }

        //On parcours le résultat en remplissant la liste
        //de Markers qui sera utilisée par les couches natives
        public void loadJson()
        {
          
                var des = (POIFromJSON)Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString2, typeof(POIFromJSON));

                foreach (POIJSON markerJson in des.data)
                {
                   
                    var marker = new MyPOI()
                    {
                        Coord = new GPSCoord() { Lat = markerJson.lattitude, Lng = markerJson.longitude },
                        Nom = markerJson.nom,
                        Type = markerJson.type,
                        Adresse = markerJson.adresse,
						Activites = markerJson.activites
                        };
                        _markerslist.Add(marker);

				}
            }

		//Lancement du l'écran de filtrage
        public ICommand GoPopupFiltre
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<FilterViewModel>());
            }
        }


		//Récupération des filtres du FilterViewModel
		public void Init(string filtreToPass)
		{
			if (filtreToPass != null && filtreToPass != "") {
				Filtre = filtreToPass.Split (',');
			} 
			else {
				Filtre = null;
			}
		}


		//Prise en compte des filtre
		public void checkFilters(){

			_markersListFiltre.Clear();

			foreach (MyPOI poi in MarkerList) {
				for (int i =0; i<Filtre.Length-1;i++) {
					if (poi.Type.Contains(Filtre[i])) {
						_markersListFiltre.Add (poi);
					}
				}
			}
		}

	}
}


