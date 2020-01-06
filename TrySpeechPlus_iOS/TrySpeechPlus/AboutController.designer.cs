// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace TrySpeechPlus
{
    [Register ("AboutController")]
    partial class AboutController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton cmdHelp { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton cmdVisitSite { get; set; }

        [Action ("cmdHelp_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void cmdHelp_TouchUpInside (UIKit.UIButton sender);

        [Action ("cmdVisitSit_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void cmdVisitSit_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (cmdHelp != null) {
                cmdHelp.Dispose ();
                cmdHelp = null;
            }

            if (cmdVisitSite != null) {
                cmdVisitSite.Dispose ();
                cmdVisitSite = null;
            }
        }
    }
}