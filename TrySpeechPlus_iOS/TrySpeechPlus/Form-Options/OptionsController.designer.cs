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
    [Register ("OptionsController")]
    partial class OptionsController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton cmdClose { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton cmdSaveBoxScore { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch optAuto { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch optFast { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch optFastEog { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch optFastEOP { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch optSpeech { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (cmdClose != null) {
                cmdClose.Dispose ();
                cmdClose = null;
            }

            if (cmdSaveBoxScore != null) {
                cmdSaveBoxScore.Dispose ();
                cmdSaveBoxScore = null;
            }

            if (optAuto != null) {
                optAuto.Dispose ();
                optAuto = null;
            }

            if (optFast != null) {
                optFast.Dispose ();
                optFast = null;
            }

            if (optFastEog != null) {
                optFastEog.Dispose ();
                optFastEog = null;
            }

            if (optFastEOP != null) {
                optFastEOP.Dispose ();
                optFastEOP = null;
            }

            if (optSpeech != null) {
                optSpeech.Dispose ();
                optSpeech = null;
            }
        }
    }
}