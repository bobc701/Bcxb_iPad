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
    [Register ("LineupCardController")]
    partial class LineupCardController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton cmdChangePos { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton cmdDoIt { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton cmdDone { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton cmdEditFielding { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton cmdMoveDown { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton cmdMoveUp { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton cmdPinchHit { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton cmdPinchRun { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton cmdReplace { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton cmdSaveLineup { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton cmdTestIt { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView dgvLineupCard { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblAction { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblGameState { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblInstructions { get; set; }

        [Action ("CmdChangePos_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CmdChangePos_TouchUpInside (UIKit.UIButton sender);

        [Action ("cmdClose_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void cmdClose_TouchUpInside (UIKit.UIButton sender);

        [Action ("cmdEditFielding_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void cmdEditFielding_TouchUpInside (UIKit.UIButton sender);

        [Action ("cmdMoveDown_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void cmdMoveDown_TouchUpInside (UIKit.UIButton sender);

        [Action ("cmdMoveUp_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void cmdMoveUp_TouchUpInside (UIKit.UIButton sender);

        [Action ("cmdPinchHit_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void cmdPinchHit_TouchUpInside (UIKit.UIButton sender);

        [Action ("cmdPinchRun_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void cmdPinchRun_TouchUpInside (UIKit.UIButton sender);

        [Action ("cmdReplace_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void cmdReplace_TouchUpInside (UIKit.UIButton sender);

        [Action ("cmdSaveLineup_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void cmdSaveLineup_TouchUpInside (UIKit.UIButton sender);

        [Action ("CmdTestIt_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CmdTestIt_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (cmdChangePos != null) {
                cmdChangePos.Dispose ();
                cmdChangePos = null;
            }

            if (cmdDoIt != null) {
                cmdDoIt.Dispose ();
                cmdDoIt = null;
            }

            if (cmdDone != null) {
                cmdDone.Dispose ();
                cmdDone = null;
            }

            if (cmdEditFielding != null) {
                cmdEditFielding.Dispose ();
                cmdEditFielding = null;
            }

            if (cmdMoveDown != null) {
                cmdMoveDown.Dispose ();
                cmdMoveDown = null;
            }

            if (cmdMoveUp != null) {
                cmdMoveUp.Dispose ();
                cmdMoveUp = null;
            }

            if (cmdPinchHit != null) {
                cmdPinchHit.Dispose ();
                cmdPinchHit = null;
            }

            if (cmdPinchRun != null) {
                cmdPinchRun.Dispose ();
                cmdPinchRun = null;
            }

            if (cmdReplace != null) {
                cmdReplace.Dispose ();
                cmdReplace = null;
            }

            if (cmdSaveLineup != null) {
                cmdSaveLineup.Dispose ();
                cmdSaveLineup = null;
            }

            if (cmdTestIt != null) {
                cmdTestIt.Dispose ();
                cmdTestIt = null;
            }

            if (dgvLineupCard != null) {
                dgvLineupCard.Dispose ();
                dgvLineupCard = null;
            }

            if (lblAction != null) {
                lblAction.Dispose ();
                lblAction = null;
            }

            if (lblGameState != null) {
                lblGameState.Dispose ();
                lblGameState = null;
            }

            if (lblInstructions != null) {
                lblInstructions.Dispose ();
                lblInstructions = null;
            }
        }
    }
}