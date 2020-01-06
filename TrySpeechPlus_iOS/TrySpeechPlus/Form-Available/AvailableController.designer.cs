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
    [Register ("AvailableController")]
    partial class AvailableController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton cmdDone { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView dgvAvailable { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblHeader { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (cmdDone != null) {
                cmdDone.Dispose ();
                cmdDone = null;
            }

            if (dgvAvailable != null) {
                dgvAvailable.Dispose ();
                dgvAvailable = null;
            }

            if (lblHeader != null) {
                lblHeader.Dispose ();
                lblHeader = null;
            }
        }
    }
}