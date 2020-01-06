// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace TrySpeechPlus
{
	[Register ("SpecialPlaysController")]
	partial class SpecialPlaysController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton cmdClose { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISwitch cmdSac { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISwitch cmdSteal { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (cmdClose != null) {
				cmdClose.Dispose ();
				cmdClose = null;
			}
			if (cmdSac != null) {
				cmdSac.Dispose ();
				cmdSac = null;
			}
			if (cmdSteal != null) {
				cmdSteal.Dispose ();
				cmdSteal = null;
			}
		}
	}
}
