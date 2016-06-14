using System;

using UIKit;
using MvvmCross.iOS.Views;
using MvvmCross.Binding.BindingContext;

namespace PocAlim.iOS
{
	public partial class TipView : MvxViewController
	{
		

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			/*
			this.CreateBinding(TipText).To((TipViewModel vm) => vm.Tip).Apply();
			this.CreateBinding(SubtotalText).To((TipViewModel vm) => vm.SubTotal).Apply();
			this.CreateBinding(GenerositySlide).To((TipViewModel vm) => vm.Generosity).Apply();
		*/
		}
	}
}


