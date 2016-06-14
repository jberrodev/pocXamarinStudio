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
            base.Start();
        }

        private List<string> _paramFiltre;

        public List<string> ParameterFiltre
        {
            get { return _paramFiltre; }
            set { _paramFiltre = value; RaisePropertyChanged(() => ParameterFiltre); }
        }

        private bool _filterRestaurantIsChecked;

        public Boolean FilterRestaurantIsChecked
        {
            get { return _filterRestaurantIsChecked; }
            set { _filterRestaurantIsChecked = value; RaisePropertyChanged(() => FilterRestaurantIsChecked); Recalculate(); }
        }
        private bool _filterProximiteIsChecked;

        public Boolean FilterProximiteIsChecked
        {
            get { return _filterProximiteIsChecked; }
            set { _filterProximiteIsChecked = value; RaisePropertyChanged(() => FilterProximiteIsChecked); Recalculate(); }
        }
        private bool _filterTransformationIsChecked;

        public Boolean FilterTransformationIsChecked
        {
            get { return _filterTransformationIsChecked; }
            set { _filterTransformationIsChecked = value; RaisePropertyChanged(() => FilterTransformationIsChecked); Recalculate(); }
        }
        private bool _filterSupermarcheIsChecked;

        public Boolean FilterSupermarcheIsChecked
        {
            get { return _filterSupermarcheIsChecked; }
            set { _filterSupermarcheIsChecked = value; RaisePropertyChanged(() => FilterSupermarcheIsChecked); Recalculate(); }
        }

        //On recharge les POI
        //en fonction des checkboxes
        private void Recalculate()
        {
            ParameterFiltre = _myFilter.Reload(FilterRestaurantIsChecked, FilterProximiteIsChecked, FilterTransformationIsChecked, FilterSupermarcheIsChecked);
        }


        public ICommand SendFiltre
        {
            get {
				return new MvxCommand (() => ShowViewModel<FillingListOfMyPOIViewModel> (new { param = ParameterFiltre.Count.ToString() }));
			}

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


