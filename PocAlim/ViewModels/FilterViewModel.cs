using System;
using System.Collections.Generic;
using System.Windows.Input;
using PocAlim.Services;
using MvvmCross.Core.ViewModels;

namespace PocAlim.ViewModels
{
    public class FilterViewModel : MvxViewModel
    {
        private readonly IMyFilter _myFilter;
        public FilterViewModel (IMyFilter filter)
        {
            _myFilter = filter;
        }


        private String _paramFiltre;

		public String ParameterFiltre
        {
            get { return _paramFiltre; }
            set { _paramFiltre = value; RaisePropertyChanged(() => ParameterFiltre); }
        }

		//boolean associé au bouton AUCUN FILTRE et OK
		private bool _aucunFiltreBool;

		private bool _filterBoucheriesCharcuteriesIsChecked;
		private bool _filterPoissonneriesIsChecked;
		private bool _filterFromageriesIsChecked;
		private bool _fitlerTraiteursIsChecked;
		private bool _filterGlaciersIsChecked;
		private bool _filterChocolatiersIsChecked;
		private bool _filterBoulangeriesPatisseriesIsChecked;
		private bool _filterAlimentationGeneraleIsChecked;
		private bool _filterSupermarchesHypermarchesIsChecked;
		private bool _filterRestaurantsIsChecked;
		private bool _filterRestaurationCollectiveIsChecked;


		public Boolean AucunFiltreBool
		{
			get { return _aucunFiltreBool; }
			set
			{
				_aucunFiltreBool = value;
				RaisePropertyChanged(() => AucunFiltreBool);
			}
		}

		public Boolean FilterBoucheriesCharcuteriesIsChecked
		{
			get { return _filterBoucheriesCharcuteriesIsChecked; }
			set
			{
				_filterBoucheriesCharcuteriesIsChecked = value;
				RaisePropertyChanged(() => FilterBoucheriesCharcuteriesIsChecked);
				Recalculate();
			}
		}

		public Boolean FilterPoissonneriesIsChecked
		{
			get { return _filterPoissonneriesIsChecked; }
			set
			{
				_filterPoissonneriesIsChecked = value;
				RaisePropertyChanged(() => FilterPoissonneriesIsChecked);
				Recalculate();
			}
		}

		public Boolean FilterFromageriesIsChecked
		{
			get { return _filterFromageriesIsChecked; }
			set
			{
				_filterFromageriesIsChecked = value;
				RaisePropertyChanged(() => FilterFromageriesIsChecked);
				Recalculate();
			}
		}

		public Boolean FilterTraiteursIsChecked
		{
			get { return _fitlerTraiteursIsChecked; }
			set
			{
				_fitlerTraiteursIsChecked = value;
				RaisePropertyChanged(() => FilterTraiteursIsChecked);
				Recalculate();
			}
		}

		public Boolean FilterGlaciersIsChecked
		{
			get { return _filterGlaciersIsChecked; }
			set
			{
				_filterGlaciersIsChecked = value;
				RaisePropertyChanged(() => FilterGlaciersIsChecked);
				Recalculate();
			}
		}

		public Boolean FilterChocolatiersIsChecked
		{
			get { return _filterChocolatiersIsChecked; }
			set
			{
				_filterChocolatiersIsChecked = value;
				RaisePropertyChanged(() => FilterChocolatiersIsChecked);
				Recalculate();
			}
		}

		public Boolean FilterBoulangeriesPatisseriesIsChecked
		{
			get { return _filterBoulangeriesPatisseriesIsChecked; }
			set
			{
				_filterBoulangeriesPatisseriesIsChecked = value;
				RaisePropertyChanged(() => FilterBoulangeriesPatisseriesIsChecked);
				Recalculate();
			}
		}

		public Boolean FilterAlimentationGeneraleIsChecked
		{
			get { return _filterAlimentationGeneraleIsChecked; }
			set
			{
				_filterAlimentationGeneraleIsChecked = value;
				RaisePropertyChanged(() => FilterAlimentationGeneraleIsChecked);
				Recalculate();

			}
		}

		public Boolean FilterSupermarchesHypermarchesIsChecked
		{
			get { return _filterSupermarchesHypermarchesIsChecked; }
			set
			{
				_filterSupermarchesHypermarchesIsChecked = value;
				RaisePropertyChanged(() => FilterSupermarchesHypermarchesIsChecked);
				Recalculate();
			}
		}

		public Boolean FilterRestaurantsIsChecked
		{
			get { return _filterRestaurantsIsChecked; }
			set
			{
				_filterRestaurantsIsChecked = value;
				RaisePropertyChanged(() => FilterRestaurantsIsChecked);
				Recalculate();
			}
		}

		public Boolean FilterRestaurationCollectiveIsChecked
        {
			get { return _filterRestaurationCollectiveIsChecked; }
			set {
				_filterRestaurationCollectiveIsChecked = value;
				RaisePropertyChanged (() => FilterRestaurationCollectiveIsChecked);
				Recalculate();
			}
        }


		public override void Start()
		{
			_filterBoucheriesCharcuteriesIsChecked = true;
			_filterPoissonneriesIsChecked = true;
			_filterFromageriesIsChecked = true;
			_fitlerTraiteursIsChecked = false;
			_filterGlaciersIsChecked = false;
			_filterChocolatiersIsChecked = false;
			_filterBoulangeriesPatisseriesIsChecked = false;
			_filterAlimentationGeneraleIsChecked = false;
			_filterSupermarchesHypermarchesIsChecked = true;
			_filterRestaurantsIsChecked = true;
			_filterRestaurationCollectiveIsChecked = false;

			Recalculate();

			base.Start();
		}

		//llistener sur le bouton aucun filtre
		public ICommand AucunFiltreBind
		{
			get
			{
				return new MvxCommand(aucunFiltreBind);
			}
		}

		public void aucunFiltreBind()
		{
			FilterBoucheriesCharcuteriesIsChecked = false;
			FilterPoissonneriesIsChecked = false;
			FilterFromageriesIsChecked = false;
			FilterTraiteursIsChecked = false;
			FilterGlaciersIsChecked = false;
			FilterChocolatiersIsChecked = false;
			FilterBoulangeriesPatisseriesIsChecked = false;
			FilterAlimentationGeneraleIsChecked = false;
			FilterSupermarchesHypermarchesIsChecked = false;
			FilterRestaurantsIsChecked = false;
			FilterRestaurationCollectiveIsChecked = false;
			AucunFiltreBool = false;
		}


        //On recharge les POI
        //en fonction des checkboxes
        private void Recalculate()
        { 
			ParameterFiltre = _myFilter.Reload(FilterBoucheriesCharcuteriesIsChecked,
			                                   FilterPoissonneriesIsChecked,
			                                   FilterFromageriesIsChecked,
			                                   FilterTraiteursIsChecked,
			                                   FilterGlaciersIsChecked,
			                                   FilterChocolatiersIsChecked,
			                                   FilterBoulangeriesPatisseriesIsChecked,
			                                   FilterAlimentationGeneraleIsChecked,
			                                   FilterSupermarchesHypermarchesIsChecked,
			                                   FilterRestaurantsIsChecked,
			                                   FilterRestaurationCollectiveIsChecked);

			//Quand il y a des filtres on active le bouton AUCUNFILTRE
			if (ParameterFiltre.Length != 0)
				AucunFiltreBool = true;
			//sinon on le desactive
			else
				AucunFiltreBool = false;

		}
			

		public ICommand SendFiltre
		{
			get 
			{
				return new MvxCommand (FunctionSend);
			}		
		}

		public void FunctionSend(){
			if(ParameterFiltre.Length != 0)
				ShowViewModel<FillingListOfMyPOIViewModel> (new { filtreToPass = ParameterFiltre});
		}
       
		public ICommand GoBack
        {
            get
            {
                return new MvxCommand(() => Close(this));
            }

        }

    }

}


