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

        public override void Start()
        {
			_filterAlimentationGeneraleIsChecked = true;
			_filterRestaurationCollectiveIsChecked = true;
			_filterSupermarchesHypermarchesIsChecked = true;
			_filterCharcuteriesIsChecked = true;

			Recalculate ();

			base.Start();
        }

		private string _errorMessage;

		public String ErrorMessage
		{
			get { return _errorMessage; }
			set { _errorMessage = value; RaisePropertyChanged(() => ErrorMessage); }
		}

        private String _paramFiltre;

		public String ParameterFiltre
        {
            get { return _paramFiltre; }
            set { _paramFiltre = value; RaisePropertyChanged(() => ParameterFiltre); }
        }

		private bool _filterRestaurationCollectiveIsChecked;

        public Boolean FilterRestaurationCollectiveIsChecked
        {
			get { return _filterRestaurationCollectiveIsChecked; }
			set {
				_filterRestaurationCollectiveIsChecked = value;
				RaisePropertyChanged (() => FilterRestaurationCollectiveIsChecked);
				Recalculate();
			}
        }
		private bool _filterAlimentationGeneraleIsChecked;

        public Boolean FilterAlimentationGeneraleIsChecked
        {
			get { return _filterAlimentationGeneraleIsChecked; }
			set {
				_filterAlimentationGeneraleIsChecked = value;
				RaisePropertyChanged (() => FilterAlimentationGeneraleIsChecked);
				Recalculate();

			}
        }
		private bool _filterCharcuteriesIsChecked;

        public Boolean FilterCharcuteriesIsChecked
        {
			get { return _filterCharcuteriesIsChecked; }
			set {
				_filterCharcuteriesIsChecked = value;
				RaisePropertyChanged (() => FilterCharcuteriesIsChecked);
				Recalculate();
			}
        }
		private bool _filterSupermarchesHypermarchesIsChecked;

        public Boolean FilterSupermarchesHypermarchesIsChecked
        {
			get { return _filterSupermarchesHypermarchesIsChecked; }
			set {
				_filterSupermarchesHypermarchesIsChecked = value;
				RaisePropertyChanged (() => FilterSupermarchesHypermarchesIsChecked);
				Recalculate();
			}
        }

        //On recharge les POI
        //en fonction des checkboxes
        private void Recalculate()
        { 
			ParameterFiltre = _myFilter.Reload(FilterRestaurationCollectiveIsChecked, FilterAlimentationGeneraleIsChecked, FilterCharcuteriesIsChecked, FilterSupermarchesHypermarchesIsChecked);
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


