
using Android.App;
using MvvmCross.Droid.Views;
using PocAlim.ViewModels;

namespace PocAlim.Droid.View
{
    /**Classe de création de la map
     * et ajout des markers**/
    [Activity(Theme = "@style/MyTheme.Popup")]
    public class FilterView : MvxActivity
    {
        //Specification du ViewModel
        public new FilterViewModel ViewModel
        {
            get { return (FilterViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
        //Une fois le ViewModel chargé on genere la vue
        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.View_Filtre);


        }
    }
}