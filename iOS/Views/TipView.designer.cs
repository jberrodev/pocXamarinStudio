// WARNING
//
// This file has been generated automatically by Xamarin Studio Enterprise to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PocAlim.iOS
{
	[Register ("TipView")]
	partial class TipView
	{
		[Outlet]
		UIKit.UISlider GenerositySlide { get; set; }

		[Outlet]
		UIKit.UITextField SubtotalText { get; set; }

		[Outlet]
		UIKit.UILabel TipText { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (GenerositySlide != null) {
				GenerositySlide.Dispose ();
				GenerositySlide = null;
			}

			if (SubtotalText != null) {
				SubtotalText.Dispose ();
				SubtotalText = null;
			}

			if (TipText != null) {
				TipText.Dispose ();
				TipText = null;
			}
		}
	}
}
