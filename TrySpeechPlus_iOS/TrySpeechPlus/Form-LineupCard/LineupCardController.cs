using Foundation;
using System;
using System.CodeDom.Compiler;
using System.IO;
using UIKit;
using BCX.BCXCommon;

namespace BCX.BCXB {

public struct CLineupChange {
   public char type;
   public int  x, y; // x replaced by y
   public int p; // Position being replaced, if applicable.
   public int q; // New position (x being moved from p to q.)
   public side abMng;

}

public partial class LineupCardController : UIViewController {

   public enum EGameState {PreGame, Offense, Defense};
   public EGameState gameState;
   public CGame g;
   public side abMng; //The side (home or vis) being managed here.
   public side abGame;    //The side currently at bat in the game
   public CLineupCard lineupCard;
   public bool pinchHitter = false;
   public bool pinchRunner = false;
   public bool newPitcher = false;
   //public UIViewController parent;


   public int SelectedRow { get; set; }
   public CBatter SelectedBatter { get; set; }

   private AvailableController fAvail; 
   private BCX.BCXCommon.CSimplePicker posPicker;

   public CLineupChange operation;    


   // Constructor:
      public LineupCardController (IntPtr handle) : base (handle) {
         // ---------------------------------------------------------
         //lineupCard = new CLineupCard(g, abMng);
         SelectedRow = -1;
      }


      public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender) {
      // -----------------------------------------------------------------------------

         base.PrepareForSegue(segue, sender);
         char crit = 'a';

         switch (operation.type) {
            case 'h':
            case 'r':
               crit = 'n'; break;
            case 's':
               if (g.UsingDh && SelectedBatter.px > 0) crit = 'p';
               else if (g.UsingDh && SelectedBatter.px == 0) crit = 'n';
               else crit = 'a';
               break;
         }

         if (segue.Identifier == "AvailableSegue_0") {   
            switch (operation.type) {
               case 'h':
               case 'r':
               case 's':
                  fAvail = segue.DestinationViewController as AvailableController;
                  lineupCard.GetAvailable(crit);
                  fAvail.lineupCard = lineupCard;
                  fAvail.g = g;
                  fAvail.abMng = abMng;
                  this.operation.abMng = abMng;
                  fAvail.operation = this.operation;
                  break;
               case 'p':
               // Change position
               break;
            }

         } 
         else if (segue.Identifier == "AvailableSegue_r") {

         // Pinch run...

         }

      }


      //int x, y;
      //string msg;


      [Action ("ReturnFromAvailable:")]
      async public void ReturnFromAvailable(UIStoryboardSegue segue) {
         // --------------------------------------------------------
         // User has closed the available dialog... (#1512.01)
         // y will repalce x...

         string msg;

         switch (fAvail.operation.type) {
            case 'h':
               // Pinch hit...
               if (fAvail.SelectedRow < 0) return;
               operation.x = lineupCard.CurrentLineup[this.SelectedRow].bx;
               operation.y = lineupCard.Available[fAvail.SelectedRow].bx;
               msg =
                  "Pinch hit " + g.t[(int)abMng].bat[operation.y].bname +
                  " for " + g.t[(int)abMng].bat[operation.x].bname;
               lblAction.Text = msg;
               cmdDoIt.Hidden = false;
               EnableControl(cmdDoIt, true);

               break;

            case 'r':
               // Pinch-run...
               if (fAvail.SelectedRow < 0) return;
               operation.x = lineupCard.CurrentLineup[this.SelectedRow].bx;
               operation.y = fAvail.SelectedBatter.bx;
               msg =
                  "Pinch run " + g.t[(int)abMng].bat[operation.y].bname +
                  " for " + g.t[(int)abMng].bat[operation.x].bname;
               lblAction.Text = msg;
               cmdDoIt.Hidden = false;
               EnableControl(cmdDoIt, true);
               break;

            case 's':
               // Defensive replacement...
               if (fAvail.SelectedRow < 0) return;
               operation.x = lineupCard.CurrentLineup[this.SelectedRow].bx;
               operation.y = fAvail.SelectedBatter.bx;

               // New player (y) goes at same posn as old (x), unless he is a pitcher,
               // then he goes in as a pitcher...
               if (g.t[(int)abMng].bat[operation.y].px != 0)
                  operation.p = 1;
               else
                  operation.p = g.t[(int)abMng].bat[operation.x].where;

               msg =
                  "Replace " + g.t[(int)abMng].bat[operation.x].bname +
                  " with " + g.t[(int)abMng].bat[operation.y].bname + "  " + CGame.PosNameLong[operation.p];
               lblAction.Text = msg;
               EnableControl(cmdDoIt, true);
               break;


            case 'u':
            // Move up...
               break;

            case 'd':
            // Move down...
               break;

            case 'f':
            // Edit fielding...
               break;

         }

      }



      // public void ShowOkAlert (
      //   string title, string msg, string lblOk, 
      //   UIViewController ctlr) {
      //// -------------------------------------------------------------------
      //   var okCanc = UIAlertController.Create (title, msg, UIAlertControllerStyle.Alert);
      //   okCanc.AddAction (UIAlertAction.Create (lblOk, UIAlertActionStyle.Default, (alert) => { }));
      //   ctlr.PresentViewController (okCanc, true, null);

      //}


      private void DoPinchHit(UIAlertAction alert) {
      // -------------------------------------------------------
         lineupCard.ReplacePlayer(operation.x, operation.y);
         lineupCard.SetLineupCard();
         //dgvLineupCard.DataSource = lineupCard.CurrentLineup;
         pinchHitter = true;
         //PerformSegue("ReturnFromAvailable", this);
         //mGame.InitBatter();
         dgvLineupCard.ReloadData();
         dgvLineupCard.Source = new CLineupCardSource((int)abMng, this);

      }


      private void DoNothing(UIAlertAction alert) {
      // -------------------------------------------------------
      // For when user cancels operation.

      }


      private string GetPosChangeMsg(int pos, int bix) {
         // -----------------------------------------------

         //CBatter bat = g.t[(int)abMng].bat[lineupCard.CurrentLineup[this.SelectedRow].bx];
         CBatter bat = g.t[(int)abMng].bat[bix];
         string msg;
         //var alert = new CAlert();
         if (gameState != EGameState.PreGame && bat.where == 10 ) {
            CAlert.ShowOkAlert("", "Can't move DH to the field", "OK", this);
            msg = "";
         }
         else if (bat.where == pos) {
            CAlert.ShowOkAlert("", "Fielder is already at position", "OK", this);
            msg = "";
         } 
         else {
            msg = "Move " + bat.bname + " to " + CGame.PosName[pos];
         }
         return msg;

      }

      private void YesAction (UIAlertAction alert) {
      // ------------------------------------------------------
         lineupCard.ReplacePlayer(operation.x, operation.y);
         lineupCard.SetLineupCard();
         dgvLineupCard.ReloadData();
         pinchHitter = true;
      }

      private void NoAction (UIAlertAction alert) {
      // ------------------------------------------------------
         pinchHitter = false;
      }


      private void ShowAlert(string title, string msg, UIViewController ctlr, int x, int y) {
         // -------------------------------------------------------------------
         var alertController = UIAlertController.Create("Pinch Hitting", msg, UIAlertControllerStyle.Alert);
         if (alertController.PopoverPresentationController != null) {
            alertController.PopoverPresentationController.SourceView = ctlr.View;
            alertController.PopoverPresentationController.SourceRect = ctlr.View.Bounds;
         }
         //alertController.AddAction(UIAlertAction.Create("Yes", UIAlertActionStyle.Default, alert => {
         //// Go ahead and make the substitution...
         //   lineupCard.ReplacePlayer(operation.x, operation.y);
         //   lineupCard.SetLineupCard();
         //   dgvLineupCard.ReloadData();
         //   pinchHitter = true;
         //}));

         //alertController.AddAction(UIAlertAction.Create("No", UIAlertActionStyle.Cancel, alert => {
         //   pinchHitter = false;
         //}));

         alertController.AddAction(UIAlertAction.Create("Yes", UIAlertActionStyle.Default, YesAction));
         alertController.AddAction(UIAlertAction.Create("No", UIAlertActionStyle.Cancel, NoAction));
         ctlr.PresentViewController(alertController, true, null);

      }


      partial void CmdTestIt_TouchUpInside (UIButton sender) {
      // --------------------------------------------------------
         cmdDoIt.Hidden = false;

         //ShowYesNoAlert( 
         //   "OK / Cancel Alert", 
         //   "This is a sample alert with an OK / Cancel Button", 
         //   "Okay", "Cancel",
         //   alert => cmdDoIt.SetTitle ("Okay", UIControlState.Normal),
         //   alert => cmdDoIt.SetTitle ("Cancel", UIControlState.Normal),
         //   this
         //);


      //// This demonstrates that even though the lambdas are sent & executed in a foriegn class,
      //// they see the local environment, namely the local vars sOkay and sCanc.   
      //   string sOkay = "Okay!";
      //   string sCanc = "Cancel!";
      //   CAlert.ShowYesNoAlert (
      //      "OK / Cancel Alert",
      //      "This is a sample alert with an OK / Cancel Button",
      //      "Okay", "Cancel",
      //      alert => cmdDoIt.SetTitle (sOkay, UIControlState.Normal),
      //      alert => cmdDoIt.SetTitle (sCanc, UIControlState.Normal),
      //      this
      //   );


         CAlert.ShowActionSheet (
            "Action Sheet sample",
            "This is a sample action sheet with 3 actions",
            new string [] { "Yes", "No", "Maybe" },
            new Action<UIAlertAction> [] {
               alert => cmdDoIt.SetTitle ("Yes", UIControlState.Normal),
               alert => cmdDoIt.SetTitle ("No", UIControlState.Normal),
               alert => cmdDoIt.SetTitle ("Maybe", UIControlState.Normal)
            },
            this
         );

      }


      partial void cmdPinchHit_TouchUpInside(UIButton sender) {
      // ---------------------------------------------------------
         operation.type = 'h';
         this.PerformSegue("AvailableSegue_0", this);

      }


      partial void cmdPinchRun_TouchUpInside(UIButton sender) {
      // ------------------------------------------------------
         operation.type = 'r';
         this.PerformSegue("AvailableSegue_0", this);

      }

      partial void cmdReplace_TouchUpInside(UIButton sender) {
      // ------------------------------------------------------
         operation.type = 's';
         this.PerformSegue("AvailableSegue_0", this);
         
      }

      partial void CmdChangePos_TouchUpInside (UIButton sender) {
         // ---------------------------------------------------------

         if (g.UsingDh && gameState == EGameState.PreGame) {
            // Allow to choose dh...   
            CAlert.ShowActionSheet(
               "Change Position",
               "Choose a new position for the selected player:",
               new string[] { "c", "1b", "2b", "3b", "ss", "lf", "cf", "rf", "dh", "Cancel" },
               new Action<UIAlertAction>[] {
               //alert => PostPosChangeMsg(1),
               alert => PostPosChangeMsg(2),
               alert => PostPosChangeMsg(3),
               alert => PostPosChangeMsg(4),
               alert => PostPosChangeMsg(5),
               alert => PostPosChangeMsg(6),
               alert => PostPosChangeMsg(7),
               alert => PostPosChangeMsg(8),
               alert => PostPosChangeMsg(9),
               alert => PostPosChangeMsg(10),
               alert => PostPosChangeMsg(-1)
               },
               this
            );
         }
         else {
            // Cannot choose dh...   
            CAlert.ShowActionSheet(
               "Change Position",
               "Choose a new position for the selected player:",
               new string[] { "c", "1b", "2b", "3b", "ss", "lf", "cf", "rf", "Cancel" },
               new Action<UIAlertAction>[] {
                  //alert => PostPosChangeMsg(1),
                  alert => PostPosChangeMsg(2),
                  alert => PostPosChangeMsg(3),
                  alert => PostPosChangeMsg(4),
                  alert => PostPosChangeMsg(5),
                  alert => PostPosChangeMsg(6),
                  alert => PostPosChangeMsg(7),
                  alert => PostPosChangeMsg(8),
                  alert => PostPosChangeMsg(9),
                  alert => PostPosChangeMsg(-1)
               },
               this
            );
         }
         
      }


      private void PostPosChangeMsg (int newPos) {
      // --------------------------------------------
         if (newPos == -1) return;
         
         operation.type = 'p';
         operation.x = lineupCard.CurrentLineup[this.SelectedRow].bx; 
         operation.p = newPos;
         operation.abMng = abMng;
         
         string msg = GetPosChangeMsg (newPos, operation.x);
         lblAction.Text = msg; //Will be empty string if errors.
         cmdDoIt.Enabled = true;

      }


      partial void cmdMoveUp_TouchUpInside(UIButton sender) {
      // ------------------------------------------------------
         int ix = SelectedRow; 
         if (ix == 0) return; //Can't move #1 batter up.
         if (ix == 9) return; //Can't move non-batting pitcher.
         int bx = lineupCard.CurrentLineup[ix].bx;
         lineupCard.MovePlayerUpDown(bx, -1); //-1 is up
         lineupCard.SetLineupCard();
         dgvLineupCard.ReloadData();
         SelectedRow = ix-1;

      }


      partial void cmdMoveDown_TouchUpInside(UIButton sender) {
      // ------------------------------------------------------
         int ix = SelectedRow; 
         if (ix == 8) return; //Can't move #9 batter down.
         if (ix == 9) return; //Can't move non-batting pitcher.
         int bx = lineupCard.CurrentLineup[ix].bx;
         lineupCard.MovePlayerUpDown(bx, 1); //+1 is down
         lineupCard.SetLineupCard();
         dgvLineupCard.ReloadData();
         SelectedRow = ix+1;

      }
      

      partial void cmdEditFielding_TouchUpInside(UIButton sender) {
      // ------------------------------------------------------
         //throw new NotImplementedException();
      }

      partial void cmdSaveLineup_TouchUpInside(UIButton sender) {
         // ------------------------------------------------------
         //DialogResult ans;

         //string msg =
         //   "This will save the current lineup as\r\n" +
         //   "the starting lineup in future games\r\n" +
         //   "(for" + (g.usingDh ? " DH games only)." : " non-DH games only).");
         //ans = MessageBox.Show(msg, "Save Lineup", MessageBoxButtons.OKCancel);
         //if (ans != DialogResult.OK) return;
         //msg = g.ValidateDefense(abMng);
         //if (msg != "") {
         //   MessageBox.Show(
         //      "Can't save lineup at this time:\r\n" + msg + "\r\n\r\n" +
         //      "(You'll be prompted to make the necesary lineup\r\n" +
         //      "change(s) when the team next takes the field.)");
         //   return;
         //}
         //g.PersistLineup((int)abMng);
         //MessageBox.Show("Lineup saved");
      }

      partial void cmdClose_TouchUpInside(UIButton sender) {
      // ------------------------------------------------------
      // Determine  operator and then do some stuff then unwind...

      }


      public override void ViewDidLoad () {
      // ----------------------------------
         base.ViewDidLoad ();
      // Perform any additional setup after loading the view, typically from a nib.

         cmdDoIt.TouchUpInside += delegate (object sender, EventArgs e) {
            // -------------------------------------------------
            try {
               switch (operation.type) {
                  case 'h':
                     lineupCard.ReplacePlayer(operation.x, operation.y);
                     lineupCard.SetLineupCard();
                     dgvLineupCard.ReloadData();
                     dgvLineupCard.Source = new CLineupCardSource((int)abMng, this);
                     CAlert.ShowOkAlert("Lineup change", "Change made!", "OK", this);
                     break;

                  case 'r':
                     lineupCard.ReplacePlayer(operation.x, operation.y);
                     lineupCard.SetLineupCard();
                     dgvLineupCard.ReloadData();
                     dgvLineupCard.Source = new CLineupCardSource((int)abMng, this);
                     CAlert.ShowOkAlert("Lineup change", "Change made!", "OK", this);
                     break;

                  case 's':
                     // Some validation...
                     CBatter bx = g.t[(int)abMng].bat[operation.x];
                     CBatter by = g.t[(int)abMng].bat[operation.y];
                     if (gameState != EGameState.PreGame && bx.where == 10) {
                        CAlert.ShowOkAlert("Lineup change", "Replacing DH is not supported. (You can pinch hit.)", "Got it", this);
                     }
                     //else if (bx.where == 1 && by.px == 0) {
                     //   CAlert.ShowOkAlert("Lineup change", "Replacing pitcher with position player is not supported.", "Got it", this);
                     //}
                     else {
                        lineupCard.ReplacePlayer(operation.x, operation.y);
                     // If new player is a p-type, he goes in as p. Else as old player's pos.... [subst.1]
                        lineupCard.AssignPos(operation.y, by.px == 1 ? 1 : operation.p);
                        lineupCard.SetLineupCard();
                        dgvLineupCard.ReloadData();
                        dgvLineupCard.Source = new CLineupCardSource((int)abMng, this);
                        CAlert.ShowOkAlert("Lineup change", "Change made!", "OK", this);
                     }
                     break;

                  case 'p':
                     // Some validation...
                     if (gameState != EGameState.PreGame && g.t[(int)abMng].bat[operation.x].where == 10) {
                        CAlert.ShowOkAlert("Lineup change", "Moving DH to the field is not supported.", "Got it", this);
                     }
                     else if (g.t[(int)abMng].bat[operation.x].where == 1) {
                        CAlert.ShowOkAlert("Lineup change", "Playing a pitcher at a position is not supported.", "Got it", this);
                     }
                     else {
                        lineupCard.AssignPos(operation.x, operation.p);
                        lineupCard.SetLineupCard();
                        dgvLineupCard.ReloadData();
                        dgvLineupCard.Source = new CLineupCardSource((int)abMng, this);
                        CAlert.ShowOkAlert("Lineup change", "Change made!", "OK", this);
                     }
                     break;
               }
            }
            catch (Exception ex) {
            // AssignPos will throw exception if it detects you are putting a
            // non-pitcher in as pitcher. (Based on his .px property being 0.)
            // I guess restricting the available list to pitchers would be
            // cleaner... future change.
            // So do nothing here.
               CAlert.ShowOkAlert("Lineup change", ex.Message, "Got it", this);
            }

            lblAction.Text = "";
            EnableControl(cmdDoIt, false);


         };


      // Read text from disk regarding instructions plus expanation
      // of fielding ratings.
      // -------------------------------------------------------------------
         using (StreamReader f = GFileAccess.GetOtherFileReader ("Strings/Instructions1.txt")) {
            lblInstructions.Text = f.ReadToEnd();
         }
         using (StreamReader f = GFileAccess.GetOtherFileReader ("Strings/Fielding1.txt")) {
            lblInstructions.Text += "\r\n" + f.ReadToEnd();
         }
         

         SetupCard();

         string msg = g.t[(int)abMng].nick;
         switch (gameState) {
            case EGameState.Offense: msg += " At Bat"; break;
            case EGameState.Defense: msg += " In Field"; break;
            case EGameState.PreGame: msg += " Pre-game"; break;
         }
         lblGameState.Text = msg;
         
         
         

//         cmdSac.Image = CSApp.Properties.Resources.GreyLight;
//         cmdSteal.Image = CSApp.Properties.Resources.GreyLight;
//         cmdIP.Image = CSApp.Properties.Resources.GreyLight;
//
//         switch (g.specialPlay) {
//            case SPECIAL_PLAY.Bunt:
//               cmdSac.Image = CSApp.Properties.Resources.CyanLight;
//               break;           
//            case SPECIAL_PLAY.Steal:
//               cmdSteal.Image = CSApp.Properties.Resources.CyanLight;
//               break;
//            case SPECIAL_PLAY.IP:
//               cmdIP.Image = CSApp.Properties.Resources.CyanLight;
//               break;
//         }


      }

      /// <summary>
      /// Sets the attributes of the buttons, etc., and their eneablement a/o visibility,.
      /// </summary>
      ///
      private void SetupCard() {
      // ----------------------------

      abGame = (side)g.ab;
         
      if (g.PlayState == PLAY_STATE.START || g.PlayState == PLAY_STATE.NONE) {
         gameState = EGameState.PreGame;
      }
      else {
         if (abGame == abMng) gameState = EGameState.Offense;
         else gameState = EGameState.Defense;
      }
         
      lineupCard = new CLineupCard(g, abMng);
      dgvLineupCard.Source = new CLineupCardSource((int)abMng, this);

      EnableControl(cmdEditFielding, false);
      EnableControl(cmdSaveLineup, false);

      cmdEditFielding.Hidden = true;
      cmdSaveLineup.Hidden = true;

      SetButtonColors(cmdMoveUp);
      SetButtonColors(cmdMoveDown);
      SetButtonColors(cmdPinchHit);
      SetButtonColors(cmdPinchRun);
      SetButtonColors(cmdReplace);
      SetButtonColors(cmdChangePos);
      SetButtonColors(cmdEditFielding);
      SetButtonColors(cmdSaveLineup);
      SetButtonColors(cmdDoIt);
      SetButtonColors (cmdDone);

      if (this.SelectedRow < 0) {
         EnableControl(cmdMoveUp, false);
         EnableControl(cmdMoveDown, false);
         EnableControl(cmdPinchHit, false);
         EnableControl(cmdPinchRun, false);
         EnableControl(cmdReplace, false);
         EnableControl(cmdDoIt, false);
         EnableControl(cmdChangePos, false);
         return;
      }

      SetLabelBorder (lblAction, UIColor.Brown, 1);

      switch (gameState) {
         case EGameState.PreGame: 
            EnableControl(cmdMoveUp, true); 
            EnableControl(cmdMoveDown, true);
            EnableControl(cmdPinchHit, false);
            EnableControl(cmdPinchRun, false);
            EnableControl(cmdReplace, true);
            EnableControl(cmdChangePos, true);
            EnableControl(cmdDoIt, false);
            break;
            
         case EGameState.Offense: 
            EnableControl(cmdMoveUp, false);
            EnableControl(cmdMoveDown, false);
            EnableControl(cmdPinchHit, false);
            EnableControl(cmdPinchRun, false);
            EnableControl(cmdReplace, false);
            EnableControl(cmdChangePos, false);
            EnableControl(cmdDoIt, false);
            break;
            
         case EGameState.Defense: 
            EnableControl(cmdMoveUp, false);
            EnableControl(cmdMoveDown, false);
            EnableControl(cmdPinchHit, false);
            EnableControl(cmdPinchRun, false);
            EnableControl(cmdReplace, true);
            EnableControl(cmdChangePos, true);
            EnableControl(cmdDoIt, false);
            break;
            
         default:
            break;
         
      }

   }


   public void RowSelected(int r, CBatter bat1) {
      // ------------------------------------------------------------------------
      // This is intended to be called by LineUpCardSource object from that
      // class's RowSelected mothod...

      if (r < 0) return; //S/n happen.
      SelectedRow = r;
      SelectedBatter = bat1;

      switch (gameState) {

         case EGameState.PreGame:
            EnableControl(cmdPinchHit, false);
            EnableControl(cmdPinchRun, false);
            EnableControl(cmdReplace, true);
            EnableControl(cmdChangePos, bat1.where != 1);
            EnableControl(cmdMoveUp, true);
            EnableControl(cmdMoveDown, true);
            break;

         case EGameState.Offense:
            EnableControl(cmdPinchHit, bat1.DisplayBase == "ab");
            EnableControl(cmdPinchRun,
               bat1.DisplayBase == "1st" ||
               bat1.DisplayBase == "2nd" ||
               bat1.DisplayBase == "3rd");
            EnableControl(cmdReplace, false);
            EnableControl(cmdChangePos, false);
            EnableControl(cmdMoveUp, false);
            EnableControl(cmdMoveDown, false);
            break;

         case EGameState.Defense:
            bool isFielder = (bat1.where >= 1 && bat1.where <= 9 || bat1.where == 0);
            EnableControl(cmdReplace, isFielder);
            EnableControl(cmdChangePos, isFielder && bat1.where != 1);
            EnableControl(cmdMoveUp, false);
            EnableControl(cmdMoveDown, false);
            break;
      }

   }


   private void SetButtonColors(UIButton btn) {
   // -------------------------------------------
      btn.BackgroundColor = UIColor.LightGray; // UIColor.LightGray;
      btn.SetTitleColor(UIColor.Blue, UIControlState.Normal);
      btn.SetTitleColor(UIColor.Gray, UIControlState.Disabled);
      
   }


   private void SetLabelBorder(UILabel lbl, UIColor color, int width) {
      // ---------------------------------------------------------------
      lbl.Layer.BorderColor = color.CGColor;
      lbl.Layer.BorderWidth = 1;

   }


   private void EnableControl (UIControl ctl, bool on) {
   // ---------------------------------------------------------------
      if (on) {
         ctl.Enabled = true;
         //btn.TitleColor(UIControlState.Normal) = UIColor.Blue;
      }
      else {
         ctl.Enabled = false;
         //btn.TitleColor(UIControlState.Disabled) = UIColor.DarkGray;
      }       
   }  


   private void EnableLabel (UILabel lbl, bool on) {
   // ---------------------------------------------------------------
      if (on) {
         lbl.Enabled = true;
         //btn.TitleColor(UIControlState.Normal) = UIColor.Blue;
      }
      else {
         lbl.Enabled = false;
         //btn.TitleColor(UIControlState.Disabled) = UIColor.DarkGray;
      }       
   }  


//   public void SetupPosPicker() {
//   // ----------------------
//
//      int selectedIx = 0;
//      string[] posList = {"p", "c", "1b", "2b", "3b", "ss", "lf", "cf", "rf"};
//      var modelBottom = new CPosPickerModel(posList);
//
//   // Instantiate the events with handlers...
//      modelBottom.PickerChanged += delegate(int p, string posAbbr) {
//         selectedIx = p;
//         this.cboPos.Text = posAbbr;
//         AssignPos(p+1);
//      };
//
//   // Setup picker view...
//      UIPickerView pickerBottom = new UIPickerView();
//      pickerBottom.ShowSelectionIndicator = true;
//      pickerBottom.Model = modelBottom;
//
////   // Set up the picker's toolbar...
////      var tb = new UIToolbar();
////      tb.BarStyle = UIBarStyle.Black;
////      tb.Translucent = true;
////      tb.SizeToFit();
//
////   // Create 'Done' button and add it to the toobar...
////      var cmdDone = new UIBarButtonItem("Done", UIBarButtonItemStyle.Done,
////         (s, e) => {
////            selectedIx = modelBottom.selectedPosIx;
////            this.cboPos.Text = modelBottom.selectedPosAbbr;
////            this.cboPos.ResignFirstResponder();
////         });
////      tb.SetItems(new UIBarButtonItem[]{cmdDone}, true);
//
//   // Tell the textbox to use the picker for input & display toolbar 
//   // over it...
//      this.cboPos.InputView = pickerBottom;
////      this.cboPos.InputAccessoryView = tb;
//          
//   }
      



    

	}
}
