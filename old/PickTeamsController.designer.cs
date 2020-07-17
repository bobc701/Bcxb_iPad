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

namespace BCX.BCXB
{
    [Register ("PickTeamsController")]
    partial class PickTeamsController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton cmdDone { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtTeamH { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtTeamV { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (cmdDone != null) {
                cmdDone.Dispose ();
                cmdDone = null;
            }

            if (txtTeamH != null) {
                txtTeamH.Dispose ();
                txtTeamH = null;
            }

            if (txtTeamV != null) {
                txtTeamV.Dispose ();
                txtTeamV = null;
            }
        }
    }
}