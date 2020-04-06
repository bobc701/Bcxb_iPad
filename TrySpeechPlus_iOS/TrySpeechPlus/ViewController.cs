using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using UIKit; 
using Foundation;
using BCX.BCXB;
using BCX.BCXCommon;
using System.Threading;
using System.Threading.Tasks;
using CoreGraphics;


namespace BCX.BCXB {
   
	public partial class ViewController : UIViewController {

      CGame mGame;
      public bool SpeechOn = true; // 4/25'17 was false
      private bool IsFieldingPlay = false;
      private GProfileDisk disk1 = null;

      CTeamRecord[] selectedTeams = new CTeamRecord[2];
      private bool pinchHitter, pinchRunner, nwPitcher;
      private LineupCardController fLineup;
      private OptionsController fOptions;
      private SpecialPlaysController fPlays; 


      // Structures that map to form elements...
      UILabel[] txtAbbrev = new UILabel[2];
      UILabel[,] grdLinescore = new UILabel[2,13];
      UILabel[,] grdRHE = new UILabel[2,3];
      UILabel[] grdInning = new UILabel[13];

      UILabel[] lblRunner = new UILabel[4]; 
      UILabel[] lblFielder = new UILabel[10]; 





		public ViewController (IntPtr handle) : base (handle) {
      // ----------------------------------------------------

		}


		public override void ViewDidLoad () {
      // ----------------------------------
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.

			var tts = new CTextToSpeach ();


         GFileAccess.SetFolders( 
            NSBundle.MainBundle.BundlePath,
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

         mGame = new CGame(); 

       //cmdGo.TouchUpInside += async delegate (object sender, EventArgs e) {
         cmdGo.TouchUpInside += delegate (object sender, EventArgs e) {
            // -------------------------------------------------
            bool clocking = false;
            BCX.BCXCommon.CActivityIndicator acty = null;
            cmdGo.TitleLabel.Text = "";
            ///cmdGo.Refresh();
            cmdGo.Enabled = false;
            cmdInfo.Enabled = false;

            if (mGame.IsFastRunMode) {
               clocking = true;
               //// Show the loading overlay on the UI thread using the correct orientation sizing...
               //acty = new BCX.BCXCommon.CActivityIndicator(UIScreen.MainScreen.Bounds);
               //View.Add(acty);
            }


          //int result = await mGame.Go1();   
            int result = mGame.Go1();  
            //*1EnableControls();
            if (clocking) acty.Hide(); //Can't rely on IsFatRunMode at end of game.


            ///txtLists.Text = mGame.ShowLists();

         };


         segVisHome.ValueChanged += delegate(object sender, EventArgs e) {
         // --------------------------------------------------------------
               SelectBoxTabs((int)segVisHome.SelectedSegment);
            
         };


         segProfileDisks.ValueChanged += delegate (object sender, EventArgs e) {
            // --------------------------------------------------------------
            //disk1 = new GProfileDisk() {Frame = new CGRect(400.0, 580.0, 300.0, 400.0)};
            //switch (segProfileDisks.SelectedSegment) {

            // First, get batter & pitcher name...

            
            switch (segProfileDisks.SelectedSegment) {
            case 0:
               if (mGame.cpara == null) return;
               disk1.Init (150, 220, mGame.cpara);
               string sBatter = mGame.CurrentBatterName;
               string sPitcher = mGame.CurrentPitcherName;
               disk1.DiceRoll = mGame.diceRollBatting;
               disk1.ProfileLabel = sBatter + " vs. " + sPitcher + ":";
               disk1.SubLabel1 = "";
               disk1.SubLabel2 = "";
               cmdInfo.Enabled = true;
               break;
            case 1:
               if (mGame.fpara == null) return;
               disk1.Init(150, 220, mGame.fpara);
               string [] aText = mGame.fpara.description.Split ('/');
               disk1.DiceRoll = mGame.diceRollFielding;
               disk1.ProfileLabel = mGame.fpara.fielderName + " fielding...";
               disk1.SubLabel1 = ""; //"Green: " + aText [0] + ", Red: " + aText [1];
               disk1.SubLabel2 = "";
               cmdInfo.Enabled = false;
               break;
            }

            disk1.SetLabelProperties(UIColor.Black.CGColor, 18f); // 18f);
            disk1.Hidden = false;
            disk1.ClearsContextBeforeDrawing = true;
            disk1.SetNeedsDisplay ();
            View.SendSubviewToBack(disk1);


            //case 0:
            //// Batting / Pitching...
            //   if (mGame.cpara == null) break;
            //   disk1.Init(150, 220, mGame.cpara);
            //   disk1.DiceRoll = mGame.diceRollBatting;
            //   disk1.ProfileLabel = sBatter + " vs. " + sPitcher + ":"; 
            //   disk1.SubLabel1 = "";
            //   disk1.SubLabel2 = "";
            //   disk1.SetLabelProperties(UIColor.Black.CGColor, 18f);
            //   disk1.Hidden = false;
            //   disk1.ClearsContextBeforeDrawing = true;
            //   disk1.SetNeedsDisplay();
            //   break;

            //case 1:
            //// Fielding...
            //   if (mGame.fpara == null) break;
            //   string[] aText = mGame.fpara.description.Split('/');
            //   disk1.Init(150, 220, mGame.fpara);
            //   disk1.DiceRoll = mGame.diceRollFielding;
            //   disk1.ProfileLabel = mGame.fpara.fielderName + " fielding...";
            //   disk1.SubLabel1 = "Green: " + aText[0] + ", Red: " + aText[1];
            //   disk1.SubLabel2 = "";
            //   disk1.SetLabelProperties(UIColor.Black.CGColor, 18f);
            //   disk1.Hidden = false;
            //   disk1.ClearsContextBeforeDrawing = true;
            //   disk1.SetNeedsDisplay();
            //   break;

         };
            

         cmdShiftLeft.TouchUpInside += delegate (object sender, EventArgs e) {
            // ------------------------------------------------------------------
            // This allows user to shift linescore 12 innings down.
            // 1706.23
            if (LinescoreStartInning > 12) {
               LinescoreStartInning -= 12;
               ShowLinescoreFull ();
            }
        };


         cmdShiftRight.TouchUpInside += delegate (object sender, EventArgs e) {
            // ------------------------------------------------------------------
            // This allows user to shift linescore 12 innings up.
            // 1706.23
            if (LinescoreStartInning < mGame.inn - 12) {
               LinescoreStartInning += 12;
               ShowLinescoreFull ();
            }
         };


      // =================================
      // Event handlers
      // =================================


         // These are the Stream fetching events. Needed because, while a PCL can 
         // process Streams, it can't open one based on file name.
         //
         mGame.ERequestModelFile += delegate (short n) {
         // --------------------------------------------------------------------
            var rdr = GFileAccess.GetModelFile(n);
            return rdr;
         };

         mGame.ERequestEngineFile += delegate (string fName) {
            // ------------------------------------------------------------------------
            // Added '.bcx' due to change in CGame. (--1909.01 Changes for team data on web.)
            string enginePath = Path.Combine(GFileAccess.ModelFolder, fName.ToLower() + ".bcx");
            //string enginePath = @"Model\" + fName;
            if (!Directory.Exists(GFileAccess.ModelFolder)) throw new FileNotFoundException();
            if (!File.Exists(enginePath)) throw new FileNotFoundException();
            var rdr = new StreamReader(enginePath);
            return rdr;
         };

         // Out 1.1.02 -- Same as BcxbXf, CGame does not request this.
         //mGame.ERequestTeamFileReader += delegate (string fName) {
         //   // --------------------------------------------------------------------
         //   var rdr = GFileAccess.GetTeamFileReader(fName);
         //   return rdr;
         //};

         mGame.ERequestTeamFileWriter += delegate (string fName) {
            // --------------------------------------------------------------------
            var rdr = GFileAccess.GetTeamFileWriter(fName);
            return rdr;
         };

         mGame.ERequestBoxFileWriter += delegate (string fName) {
            // --------------------------------------------------------------------
            var rdr = new StreamWriter(fName);
            return rdr; 
         };


         // Now that event handlers are instntiated, do this...
         mGame.SetupEngineAndModel();

         // These are the screen updating events. CGame will fire
         // these as needed and we will respond...
       //mGame.EShowRunners += ShowRunners;
         mGame.EShowRunners += delegate() { }; //Handle this is EShowResults
       //mGame.EShowRunnersOnly += ShowRunnersOnly;
         mGame.EShowRunnersOnly += delegate() { }; //Handle this is EShowResults
         mGame.EShowFielders += ShowFielders;
         mGame.ESelectBoxTabs += SelectBoxTabs;
       //mGame.EPostOuts += PostOuts;
         mGame.EPostOuts += delegate() { }; //Handle this is EShowResults
         //mGame.EUpdateBBox += UpdateBBox;
         //mGame.EUpdatePBox += UpdatePBox;
         mGame.ERefreshBBox += RefreshBBox;
         mGame.ERefreshPBox += RefreshPBox;
         mGame.EShowLinescore += ShowLinescoreOne;
         mGame.EShowLinescoreFull += ShowLinescoreFull; 
         mGame.EInitLinescore += InitLinescore;
         mGame.EShowRHE += ShowRHE;


         mGame.EShowResults += async delegate (int scenario) {
            /* -------------------------------------------------------------------
             *  There are 2 parts to ShowResults -- The part in Cgame, whatever it 
             *  wants to do with the msg, like keep a scrolling log, and then the 
             *  hand-off to the client, namely the form, which will display it in 
             *  the text box. This is the client part.
             *  
             *  We might add a scrolling store of whole-game play by play here.
             *  Possibly a List of string.
             */
            //SpeechSynthesizer s = new SpeechSynthesizer();
            //s.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Adult)

            //if (mGame.IsFastRunMode) {
            //   mGame.lstResults.Clear();
            //   return;
            //}

            TextToSay[] list1 = mGame.lstResults.ToArray();

            if (!mGame.IsFastRunMode) { 
               foreach (var txt in list1) {
                  if (txt.action == 'X') {
                     txtResults.Text = "";
                     await Task.Delay(100);
                  }
                  else {
                     if (txtResults.Text == "") txtResults.Text = txt.msg;
                     else txtResults.Text += txt.delim + txt.msg;
                     if (SpeechOn && !mGame.IsFastRunMode && txt.msg != "...") {
                        //tts.Speak(txt.msg); //#1604.03
                        var s = new System.Text.StringBuilder(txt.msg);
                        s.Replace("0 out", "none out");
                        s.Replace(" 0", " nothing");
                        if (s[1] == '.') s.Remove(0, 2); //Don't say player's initial
                        tts.Speak(s.ToString());
                     }
                     if (txt.delay)
                        await Task.Delay(mGame.runMode == CGame.RunMode.FastEOP ? 100 : 1200); 
                     else
                        await Task.Delay(100);
                  }
               }
            }
            else {
               string s = "";
               foreach (var txt in list1) {
                  if (txt.action == 'X') {
                     s = "";
                  }
                  else {
                     if (s == "") s = txt.msg;
                     else s += txt.delim + txt.msg;
                  }
               }
               txtResults.Text = s;
               await Task.Delay(100);
            }
            mGame.lstResults.Clear();

         // #1510.01:
         // Run these here
            switch (scenario) {
               case 1: ShowRunners(); segProfileDisks.SetEnabled(false, 1); cmdInfo.Enabled = true; break;
               case 2: ShowRunnersOnly(); break;
            }
            PostOuts();
            await Task.Delay(100);

         // Here is where to show the disk...
            string sBatter= mGame.CurrentBatterName;
            string sPitcher= mGame.CurrentPitcherName;

            disk1.Init(150, 220, mGame.cpara);
            disk1.DiceRoll = mGame.diceRollBatting;
            disk1.ProfileLabel = sBatter + " vs. " + sPitcher + ":";
            disk1.SubLabel1 = "";
            disk1.SetLabelProperties(UIColor.Black.CGColor, 18f); // 18f);
            disk1.Hidden = false;
            disk1.ClearsContextBeforeDrawing = true;
            disk1.SetNeedsDisplay();
            cmdInfo.Enabled = true;

            EnableControls (); //*1

//            var s = txtResults.Text;
//            if (s == "") {
//               txtResults.Text = s; 
//               Task.Delay(1500);
//            }
//            else 
//               txtResults.Text += delim + s;
//            if (SpeechOn && !mGame.IsFastRunMode) voice.Speak(msg); //#1505.02
 
            ///txtResults.Refresh();
         };



         mGame.EUpdateBoxes += async delegate () {
         // -------------------------------------------------------------------
            StatToUpdate[] list1 = mGame.lstBoxUpdates.ToArray();
            if (!mGame.IsFastRunMode) {
               foreach (var stat in list1) {
                  switch (stat.action) {
                     case 'B': UpdateBBox(stat.newValue, stat.sc, stat.ab, stat.ix); break;
                     case 'P': UpdatePBox(stat.newValue, stat.sc, stat.ab, stat.ix); break;
                  }
                  await Task.Delay(100);
               }
            }
            mGame.lstBoxUpdates.Clear();

         };


         mGame.EClearResults += async() => {
         // -------------------------------------------------------------------
            //if (mGame.IsFastRunMode) return;
            txtResults.Text = "";
            await Task.Yield();
            ///txtResults.Refresh();

         };


         mGame.EFmtParamBar += delegate() {
         // ---------------------------------------------------------------------
            if (mGame.runMode == CGame.RunMode.Normal) { 
               //fbarMain.Hide();
//               int i = mGame.linup[mGame.ab, mGame.slot[mGame.ab]];
//               int j = mGame.curp[mGame.fl];
//               CBatter b = mGame.bat[mGame.ab,i];
//               CPitcher p = mGame.pit[mGame.fl,j];
//               string sBatter= b.bname;
//               string sPitcher= p.pname;

            // The old way using parambar...
               //pbarMain.FmtBarSegments(mGame.cpara, 16, this.Width, txtHR.Top);
               //pbarMain.UpdPhysicalDescriptor(sPitcher + " pitching to " + sBatter + ":");

            // The new way using disks... 
            // Code has been moved to ...
               //draw4 = new GProfileDisk (550, 650, mGame.cpara, 1, 7 ){Frame = UIScreen.MainScreen.Bounds};
//               draw4.Init(150, 200, mGame.cpara, 1, 7);
//               draw4.DiceRoll = mGame.diceRollBatting;
//               draw4.ProfileLabel = sBatter + " vs. " + sPitcher + ":"; 
//               draw4.SetLabelProperties(UIColor.Black.CGColor, 18f);
//               draw4.Hidden = false;
//               draw4.ClearsContextBeforeDrawing = true;
//               draw4.SetNeedsDisplay();


               IsFieldingPlay = false;
            }
            else {
               //pbarMain.Hide();
               //fbarMain.Hide();
            }
         };


         mGame.EFmtFieldingBar += delegate(
            CFieldingParamSet fPar, string labels, string fielderName) {
         // ---------------------------------------------------------------------
            if (mGame.runMode == CGame.RunMode.Normal) { 
               //pbarMain.Hide();
               //fbarMain.FmtBarSegments(fPar, 16, this.Width, txtHR.Top);
               //fbarMain.SetBarLabels(labels.Split('/'));
               //fbarMain.UpdPhysicalDescriptor(fielderName + " fielding...");
               cmdInfoFielding.Hidden = false;
               segProfileDisks.SetEnabled(true, 1); 
               cmdInfo.Enabled = false;
            }
            else {
               //pbarMain.Hide();
               //fbarMain.Hide();
               //fbarMain.HidePhysicalDescriptor();
               segProfileDisks.SetEnabled(false, 1);
               cmdInfo.Enabled = false;
            }
         };


         mGame.EPlaceDicePointer += delegate(CDiceRoll diceRoll, bool vis) {
         // ---------------------------------------------------------------------
            if (IsFieldingPlay) return; //Do not show batting pointer if thisis a fielding play.
            //Debug.Print("In CGame.PlaceDicePointer(" + 
            //   diceRoll.pointInBracket.ToString() + "," + diceRoll.topLevelResult.ToString());
            //pbarMain.PlaceDicePointer(diceRoll, vis);
         };


         mGame.EPlaceFldgPointer += delegate(CDiceRoll diceRoll, bool vis) {
            // ---------------------------------------------------------------------
            //Debug.Print( 
            //   "In CGame.PlaceFldgPointer(" + diceRoll.pointInBracket.ToString() + "," + 
            //   diceRoll.topLevelResult.ToString());
            //fbarMain.PlaceDicePointer(diceRoll, vis);
            IsFieldingPlay = true;
         };


         mGame.EHideDicePointer += delegate() {
         // ---------------------------------------------------------------------
            //Debug.Print("In CGame.HideDicePointer()");
            //pbarMain.physicalPointer.Visible = false;
            cmdInfoFielding.Hidden = true;
         };


         mGame.EHighlightBBox += delegate(int ab, int bx) {
         // ----------------------------------------------------------------------
         // New in #1506.01
         // Purpose: Highlight batter name in box score.

//            DataGridView dgv = (ab == 0) ? dgvBV : dgvBH;
//            int bboxix = mGame.bat[ab, bx].bbox;
//            dgv.Rows[bboxix-1].Cells[0].Selected = true;
//            dgv.Refresh();

         };


         mGame.EHighlightPBox += delegate(int fl, int px)  {
            // ----------------------------------------------------------------------
            // New in #1506.01
            // Purpose: Highlight pitcher name in box score.

//            DataGridView dgv = (fl == 0) ? dgvPV : dgvPH;
//            int pboxix = mGame.pit[fl, px].pbox;
//            dgv.Rows[pboxix - 1].Cells[0].Selected = true;
//            dgv.Refresh();

         };
         
         
         mGame.ENotifyUser += delegate(string s) {
            // ---------------------------------------------------------------------
            //var alert1 = new CAlert(); 
            CAlert.ShowOkAlert("", s, "OK", DoStuff, this);

         };


         SetupScreen();


//			cmdListVoices.TouchUpInside += (object sender, EventArgs e) => {
//         // -------------------------------------------------------------
//				txtVoiceList.Text = voice.GetVoices();
//
//			};
//
//			cmdSpeak.TouchUpInside += (object sender, EventArgs e) => {
//         // --------------------------------------------------------
//				voice.Speak("Ground ball to third!");
//
//			};

		}

      private void DoStuff(UIAlertAction alert) {

      }


      public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender) {
         // -----------------------------------------------------------------------------

         base.PrepareForSegue(segue, sender);


         // set the View Controller that’s powering the screen we’re
         // transitioning to

         // Here we would pass any data that needs to go to the segue's screen...
         //          if (tryDrawContoller != null) {
         //            callHistoryContoller.PhoneNumbers = PhoneNumbers;
         //          }



         if (segue.Identifier == "ProfileDiskSegue") {        
            // ----------------------------------------
            var tryDrawContoller = segue.DestinationViewController as TryDrawController;
            //tryDrawContoller.g = mGame;
            tryDrawContoller.AssignGame(mGame);
         } 

         else if (segue.Identifier == "PickTeamsSegue") {
            // ---------------------------------------------
            try {
               var pickTeamsController = segue.DestinationViewController as PickTeamsController;
               pickTeamsController.selectedTeams = selectedTeams; //When user clicks 'Done' on PickTeams, this comes back filled.
            }
            catch (Exception ex) {
               CAlert.ShowOkAlert("Error", "Error reading data from the Internet: " + ex.Message, "OK", this);
            }
         } 

         else if (segue.Identifier == "LineupCardSegueV") {
         // -----------------------------------------------
         // #1512.01            
            fLineup = segue.DestinationViewController as LineupCardController; 
         // In Windows, this is in constructor...
            fLineup.g = mGame; 
            fLineup.operation.abMng = side.vis;
            fLineup.lineupCard = new CLineupCard(mGame, side.vis); 
         //fLineup.parent = this;

         // Return stuff happens in ReturnFromLinupCardSegueV.

         } 

         else if (segue.Identifier == "LineupCardSegueH") {
         // -----------------------------------------------
            fLineup = segue.DestinationViewController as LineupCardController; 
         // In Windows, this is in constructor...
            fLineup.g = mGame; 
            fLineup.abMng = side.home;
            fLineup.lineupCard = new CLineupCard(mGame, side.home); 

         // Return stuff happens in ReturnFromLinupCardSegueH.

         } 

         else if (segue.Identifier == "OptionsSegue") {
         // -------------------------------------------------
            fOptions = segue.DestinationViewController as OptionsController; 
            fOptions.gm = mGame;
            fOptions.RunMode = mGame.runMode;
            fOptions.SpeechOn = this.SpeechOn;

         // Return stuff happens in ReturnFromOptionsSegue.
         }

         else if (segue.Identifier == "SpecialPlaysSegue") {
         // -------------------------------------------------
            fPlays = segue.DestinationViewController as SpecialPlaysController;  
            fPlays.g = mGame;

         // Return stuff happens in ReturnFromSpecialPlaysSegue.
         }

      }
     

      [Action ("ReturnFromOptions:")]
      public void ReturnFromOptions(UIStoryboardSegue segue) {
      // --------------------------------------------------------
      // This executes after Unwind Segue from OptionsController...
         mGame.runMode = fOptions.RunMode;
         this.SpeechOn = fOptions.SpeechOn;

      }


      [Action ("ReturnFromPickTeams:")]
      public void ReturnFromPickTeams(UIStoryboardSegue segue) {
      // --------------------------------------------------------
         Console.WriteLine("We've returned from PickTeams!");
         Console.WriteLine("Vititing team: " + this.selectedTeams[0].TeamTag);
         Console.WriteLine("Home team: " + this.selectedTeams[1].TeamTag);
         // Fill mGame here.
         // Question: Study CSApp -- what happens following FPickTeams???

         SetupNewGame();
         txtResults.Text =
            "Tap 'Manage' buttons, above, to change starting lineups.\r\n" +
            "When done, tap 'Start' below.";
            

      }


      [Action ("ReturnFromPickTeamsCanc:")]
      public void ReturnFromPickTeamsCanc (UIStoryboardSegue segue) {
         // --------------------------------------------------------
         Console.WriteLine ("We've returned from PickTeams with 'Cancel'");

      }
      
      
      [Action ("ReturnFromSpecialPlays:")]
      public void ReturnFromSpecialPlays(UIStoryboardSegue segue) {
      // --------------------------------------------------------
         Console.WriteLine("We've returned from SpecialPlays!");

         mGame.specialPlay = fPlays.Play;

      }
           
      [Action ("ReturnFromLineupCardV:")]
      public void ReturnFromLineupCardV(UIStoryboardSegue seque) {
         // ---------------------------------------------------------
         // #1512.01
         Console.WriteLine("We've returned from LineupCard (V or H)!");
         // Stuff that, in Win, is after ShowModel, in button _Click...
         // Note: This handles both the ManageV and ManageH buttons,
         // (because not sure how to 'unwind' to 2 places.)

         int x = fLineup.operation.x,
         y = fLineup.operation.y,
         p = fLineup.operation.p,
         q = fLineup.operation.q;

         switch (fLineup.operation.type) {
            case 'h': //Pinch hit...
               //fLineup.lineupCard.ReplacePlayer(x, y);
               fLineup.lineupCard.SetLineupCard();
               fLineup.pinchHitter = true;
               mGame.InitBatter();
               break;
            
            case 'r': //Pinch run...
               //fLineup.lineupCard.ReplacePlayer(x, y);
               fLineup.lineupCard.SetLineupCard();
               fLineup.pinchRunner = true;
               break;

            case 's':
               //fLineup.lineupCard.ReplacePlayer(x, y);  
               //fLineup.lineupCard.AssignPos(y, p);
               //fLineup.lineupCard.SetLineupCard();
               if (p == 1)
                  fLineup.newPitcher = true;
               break;

            case 'p':
               //fLineup.lineupCard.AssignPos(x, p);
               fLineup.lineupCard.SetLineupCard();
               break;
            

         }
         cmdGo.Enabled = true;

         RefreshBBox((int)fLineup.operation.abMng);
         RefreshPBox((int)fLineup.operation.abMng);

         ShowRunners();
         ShowFielders(mGame.fl);
         //if (fLineup.pinchHitter || fLineup.pinchRunner || fLineup.newPitcher) {mGame.InitBatter();}
         fLineup.Dispose();
        
      }


      [Action ("ReturnFromLineupCardCancel:")]
      public void ReturnFromLineupCardCancel(UIStoryboardSegue seque) {
      // ---------------------------------------------------------
      // #1512.01
         Console.WriteLine("We've returned from LineupCard with Cancel!");
         //var alert1 = new CAlert();
         CAlert.ShowOkAlert("Return", "Action was cancelled", "OK", this);
         fLineup.Dispose();

      }


      [Action ("ReturnFromLineupCardH:")]
      public void ReturnFromLineupCardH(UIStoryboardSegue seque) {
      // ---------------------------------------------------------
         Console.WriteLine("We've returned from LineupCard (H)!");
      // Stuff that, in Win, is after ShowModel, in button _Click...
      // Not used <--------
         
         RefreshBBox((int)side.home);
         RefreshPBox((int)side.home);
         ShowRunners();
         ShowFielders(mGame.fl);
         if (fLineup.pinchHitter || fLineup.pinchRunner || fLineup.newPitcher) {mGame.InitBatter();}
         fLineup.Dispose();

      }


      private void SetupNewGame() {
      // --------------------------------------------------------
      // Have just returned from PickTeams, and have the selected teams
      // in selectedTeams[] which is array of CTeamRecord...

         string fName0, fName1;
         fName0 = selectedTeams[0].TeamTag; 
         fName1 = selectedTeams[1].TeamTag; 
         //mGame.UsingDh = selectedTeams[1].UsesDh; 
                
         if ((fName0 ?? "") == "" || (fName1 ?? "") == "") return; //User did not pick.       
           
      // User has selected 2 teams for new game.
      // So set up the two CTeam objects for the 2 teams...
         mGame.t = new CTeam[2];
         mGame.t[0] = new CTeam(mGame);
         mGame.t[1] = new CTeam(mGame);
            
         mGame.cmean = new CHittingParamSet(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
         mGame.PlayState = PLAY_STATE.START;

      // ------------------------------------------------
      // Section copied from BcxbXf for fetching from Web 
      // --1909.01 Changes for team data on web.
      // ------------------------------------------------
         try {

            using (StringReader f = GFileAccess.GetTextFileOnLine(fName1 + ".bcxt")) {
               //using (StringReader f = (StringReader)mGame.GetTeamFileReader(newTeams[1])) {
               mGame.t[1].ReadTeam(f, 1);
               f.Close();
            }
         }
         catch (Exception ex) {
            string msg = "Error reading " + fName1 + " team data from Internet: \r\n" + ex.Message;
            throw new Exception(msg);
         }

         try {
            using (StringReader f = GFileAccess.GetTextFileOnLine(fName0 + ".bcxt")) {
               //using (StringReader f = (StringReader)mGame.GetTeamFileReader(newTeams[0])) {
               mGame.t[0].ReadTeam(f, 0);
               f.Close();
            }
         }
         catch (Exception ex) {
            string msg = "Error reading " + fName0 + " team data from Internet: \r\n" + ex.Message;
            throw new Exception(msg);
         }
      // ------------------------------------------------


         mGame.cmean.CombineLeagueMeans(mGame.t[0].lgMean, mGame.t[1].lgMean);

         mGame.InitGame();
         ShowRHE();
         InitLinescore();
         ShowFielders(1);
         ShowRunners();

         EnableControls();
         //This seems to revert to Storyboard when returning w/ UW segue. So skip it...
         //cmdManageH.TitleLabel.Text = "Manage " + mGame.t[1].nick;
         //cmdManageV.TitleLabel.Text = "Manage " + mGame.t[0].nick; 
         ///this.tbcB.TabPages[0].Text = this.tbcP.TabPages[0].Text = mGame.nick[0];
         ///this.tbcB.TabPages[1].Text = this.tbcP.TabPages[1].Text = mGame.nick[1];


      }


      private void SetupScreen() {
      // --------------------------------------------

      // =====================================
      // Fielding and base runner display area
      // =====================================
         lblRunner[1] = lblRunner1;
         lblRunner[2] = lblRunner2;
         lblRunner[3] = lblRunner3;

         lblFielder[1] = lblFielder1; 
         lblFielder[2] = lblFielder2;
         lblFielder[3] = lblFielder3;
         lblFielder[4] = lblFielder4;
         lblFielder[5] = lblFielder5;
         lblFielder[6] = lblFielder6;
         lblFielder[7] = lblFielder7;
         lblFielder[8] = lblFielder8;
         lblFielder[9] = lblFielder9;


      // ================================
      // Linescore setup
      // ================================
      // Point internal structures at form structures for line score...
         txtAbbrev[0] = txtAbbrev0;
         txtAbbrev[1] = txtAbbrev1;

         grdLinescore[0,1] = grdLinescore001;
         grdLinescore[0,2] = grdLinescore002;
         grdLinescore[0,3] = grdLinescore003;
         grdLinescore[0,4] = grdLinescore004;
         grdLinescore[0,5] = grdLinescore005;
         grdLinescore[0,6] = grdLinescore006;
         grdLinescore[0,7] = grdLinescore007;
         grdLinescore[0,8] = grdLinescore008;
         grdLinescore[0,9] = grdLinescore009;
         grdLinescore[0,10] = grdLinescore010;
         grdLinescore[0,11] = grdLinescore011;
         grdLinescore[0,12] = grdLinescore012;

         grdLinescore[1,1] = grdLinescore101;
         grdLinescore[1,2] = grdLinescore102;
         grdLinescore[1,3] = grdLinescore103;
         grdLinescore[1,4] = grdLinescore104;
         grdLinescore[1,5] = grdLinescore105;
         grdLinescore[1,6] = grdLinescore106;
         grdLinescore[1,7] = grdLinescore107;
         grdLinescore[1,8] = grdLinescore108;
         grdLinescore[1,9] = grdLinescore109;
         grdLinescore[1,10] = grdLinescore110;
         grdLinescore[1,11] = grdLinescore111;
         grdLinescore[1,12] = grdLinescore112;

         grdRHE[0,0] = grdRHE00;
         grdRHE[0,1] = grdRHE01;
         grdRHE[0,2] = grdRHE02;
         grdRHE[1,0] = grdRHE10;
         grdRHE[1,1] = grdRHE11;
         grdRHE[1,2] = grdRHE12;

         grdInning[1] = grdInning01;
         grdInning[2] = grdInning02;
         grdInning[3] = grdInning03;
         grdInning[4] = grdInning04;
         grdInning[5] = grdInning05;
         grdInning[6] = grdInning06;
         grdInning[7] = grdInning07;
         grdInning[8] = grdInning08;
         grdInning[9] = grdInning09;
         grdInning[10] = grdInning10;
         grdInning[11] = grdInning11;
         grdInning[12] = grdInning12;

         // Position the profile disk below the toggle bar
         // ------------------------------------------------------------------------
         // X & Y are upper left.
         //disk1 = new GProfileDisk() {Frame = new CGRect(400.0, 580.0, 300.0, 400.0)};
         double x1; // = segProfileDisks.Frame.X; These settings seem to be pre-constraint?
         double y1; // = segProfileDisks.Frame.Y + segProfileDisks.Frame.Height + 10;
         x1 = View.Center.X + 15; // This works -- trial & error.
         y1 = View.Center.Y + 75;
         disk1 = new GProfileDisk() { Frame = new CGRect(x1, y1, 300.0, 400.0) };
         
         disk1.DiceRoll = mGame.diceRollBatting;
         disk1.ProfileLabel = "";
         disk1.SetLabelProperties(UIColor.Black.CGColor, 18f); // 18f);
         disk1.Opaque = false;
         disk1.Hidden = true;
         View.AddSubview (disk1);
         View.SendSubviewToBack(disk1);

         EnableControls();


      }


      public void ShowRHE() {
      // -------------------
      // grdRHE converted to an array in V1
         grdRHE[0,0].Text = mGame.rk[0,0].ToString();
         grdRHE[1,0].Text = mGame.rk[1,0].ToString();
         grdRHE[0,1].Text = mGame.rk[0,1].ToString();
         grdRHE[1,1].Text = mGame.rk[1,1].ToString();
         grdRHE[0,2].Text = mGame.rk[0,2].ToString();
         grdRHE[1,2].Text = mGame.rk[1,2].ToString();

         for (int i=0;i<=1;i++) for (int j=0;j<=2;j++) {
            ///grdRHE[i,j].Refresh();
         }

      }


      public void ShowRunners() {
      // --------------------------------------------------------------
         //if (mGame.IsFastRunMode) return;
         for (int b=1; b<=3; b++) {
            lblRunner[b].Text = mGame.r[b].name;
            ///lblRunner[b].Refresh();
         }
         lblBatter.Text = mGame.r[0].name; lblBatter.Hidden = false;
         ///lblBatter.Refresh();
         ///this.Refresh();
      }


      public void ShowRunnersOnly() {
         // --------------------------------------------------------------
         // This version of ShowRunners leaves the batter blank.
         //if (mGame.IsFastRunMode) return;
         for (int b = 1; b <= 3; b++) { lblRunner[b].Text = mGame.r[b].name; lblRunner[b].Hidden = false; }
         lblBatter.Text = ""; lblBatter.Hidden = false;
         ///lblBatter.Refresh();
         ///this.Refresh();
      }


      public void ShowFielders(int fl) {
      // --------------------------------------------------------------
         for (int p = 1; p <= 9; p++) { 
            if (mGame.t[fl].who[p] > 0)
               lblFielder[p].Text = mGame.t[fl].bat[mGame.t[fl].who[p]].bname;
            else
               lblFielder[p].Text = "???";
            lblFielder[p].Hidden = false;
            ///lblFielder[p].Refresh();
         }
      }


      public void SelectBoxTabs(int ab) {
      // --------------------------------------------------------------
         tblBoxScoreV.Hidden = (ab == 1);
         tblBoxScoreH.Hidden = (ab == 0);

         //switch (ab) {
         //   case 0: tpgBV.Select(); tpgPH.Select(); tbcB.SelectTab("tpgBV"); break;
         //   case 1: tpgBH.Select(); tpgPV.Select(); break;
         //}
//         switch (ab) {
//            case 0: tbcB.SelectTab(0); tbcP.SelectTab(1); break;
//            case 1: tbcB.SelectTab(1); tbcP.SelectTab(0); break;
//         }

      }


      public void PostOuts() {
         // --------------------------------------------------------------
         //         lblOuts1.Image  = pboxRed.Image;
         //
         // Set the Outs indicators...


         //#1812...
         //Changed this ito use image sets in the Images asset catalog...
         //The arg to FromBundle is the name of the image set.
         
         //lblOuts1.Image = mGame.ok>0 ? UIImage.FromBundle("Images/redBall2.png") : UIImage.FromBundle("Images/whtBall2.png");   
         //lblOuts2.Image = mGame.ok>1 ? UIImage.FromBundle("Images/redBall2") : UIImage.FromBundle("Images/whtBall2"); 
         //lblOuts3.Image = mGame.ok>2 ? UIImage.FromBundle("Images/redBall2.png") : UIImage.FromBundle("Images/whtBall2.png"); 
         lblOuts1.Image = mGame.ok > 0 ? UIImage.FromBundle("RedBall") : UIImage.FromBundle("WhtBall");
         lblOuts2.Image = mGame.ok > 1 ? UIImage.FromBundle("RedBall") : UIImage.FromBundle("WhtBall");
         lblOuts3.Image = mGame.ok > 2 ? UIImage.FromBundle("RedBall") : UIImage.FromBundle("WhtBall");

         //View.SendSubviewToBack(lblOuts); //Try this so out balls are visible on device.
         //View.BringSubviewToFront(lblOuts1); 
         //View.BringSubviewToFront(lblOuts2);
         //View.BringSubviewToFront(lblOuts3);

         //         lblOuts1.Refresh();
         //         lblOuts2.Refresh();
         //         lblOuts3.Refresh();

      }


      public void RefreshBBox(int ab) {
         // --------------------------------------------------------------
         // dataGridView1.Rows[3].Cells["Name1"].Value = "Nixon"; 
         // dataGridView1.Rows[3].Cells["Name1"].Style.ForeColor =
         //    System.Drawing.Color.Blue; 
         // --------------------------------------------------------------


      // Update the entire table...
         switch (ab) {
            case 0:
               tblBoxScoreV.ReloadData();
               tblBoxScoreV.Source = new CBoxScoreSource(mGame, 0);
               break;
            case 1:
               tblBoxScoreH.ReloadData();
               tblBoxScoreH.Source = new CBoxScoreSource(mGame, 1);
               break;
         }



//         int bx, j;
//         CBatter b;
//         switch (ab) {
//            case 0: dgvBV.Rows.Clear(); break;
//            case 1: dgvBH.Rows.Clear(); break;
//         }
//         for (int i=1; i < CGame.SZ_BAT; i++) {
//            if ((bx = mGame.xbox[ab,i]) == 0) continue; 
//            b = mGame.bat[ab,bx];
//            switch (ab) {
//               case 0:
//                  dgvBV.Rows.Add();
//                  j = dgvBV.Rows.Count - 1;
//                  dgvBV.Rows[j].Cells["colBVname"].Value = b.bs.boxName;
//                  dgvBV.Rows[j].Cells["colBVab"].Value = b.bs.ab.ToString();
//                  dgvBV.Rows[j].Cells["colBVr"].Value= b.bs.r.ToString();
//                  dgvBV.Rows[j].Cells["colBVh"].Value= b.bs.h.ToString();
//                  dgvBV.Rows[j].Cells["colBVrbi"].Value= b.bs.bi.ToString();
//                  dgvBV.Rows[j].Cells["colBVb2"].Value= b.bs.b2.ToString();
//                  dgvBV.Rows[j].Cells["colBVb3"].Value= b.bs.b3.ToString();
//                  dgvBV.Rows[j].Cells["colBVhr"].Value= b.bs.hr.ToString();
//                  dgvBV.Rows[j].Cells["colBVbb"].Value= b.bs.bb.ToString();
//                  dgvBV.Rows[j].Cells["colBVso"].Value= b.bs.so.ToString();
//                  break;
//               case 1:
//                  dgvBH.Rows.Add();
//                  j = dgvBH.Rows.Count - 1;
//                  dgvBH.Rows[j].Cells["colBHname"].Value = b.bs.boxName;
//                  dgvBH.Rows[j].Cells["colBHab"].Value = b.bs.ab.ToString();
//                  dgvBH.Rows[j].Cells["colBHr"].Value= b.bs.r.ToString();
//                  dgvBH.Rows[j].Cells["colBHh"].Value= b.bs.h.ToString();
//                  dgvBH.Rows[j].Cells["colBHrbi"].Value= b.bs.bi.ToString();
//                  dgvBH.Rows[j].Cells["colBHb2"].Value= b.bs.b2.ToString();
//                  dgvBH.Rows[j].Cells["colBHb3"].Value= b.bs.b3.ToString();
//                  dgvBH.Rows[j].Cells["colBHhr"].Value= b.bs.hr.ToString();
//                  dgvBH.Rows[j].Cells["colBHbb"].Value= b.bs.bb.ToString();
//                  dgvBH.Rows[j].Cells["colBHso"].Value= b.bs.so.ToString();
//                  break;
//            }
//         }
//         dgvBV.Refresh();
//         dgvBH.Refresh();
      }


      public void RefreshPBox(int fl) {
      // ----------------------

         // Update the entire table...
         switch (fl) {
            case 0:
               tblBoxScoreV.ReloadData();
               tblBoxScoreV.Source = new CBoxScoreSource(mGame, 0);
               break;
            case 1:
               tblBoxScoreH.ReloadData();
               tblBoxScoreH.Source = new CBoxScoreSource(mGame, 1);
               break;
         }



//         int i = 1, px;
//         CPitcher p;
//         switch (fl) {
//            case 0: dgvPV.Rows.Clear(); break;
//            case 1: dgvPH.Rows.Clear(); break;
//         }
//         while ((px = mGame.ybox[fl, i]) != 0)
//         {
//            p = mGame.pit[fl,px];
//            switch (fl) {
//               case 0:
//                  dgvPV.Rows.Add();
//                  dgvPV.Rows[i-1].Cells["colPVname"].Value = p.pname;
//                  dgvPV.Rows[i-1].Cells["colPVip"].Value = CGame.StatDisplayStr(p.ps.ip3, StatCat.ip);
//                  dgvPV.Rows[i-1].Cells["colPVr"].Value = p.ps.r.ToString();
//                  dgvPV.Rows[i-1].Cells["colPVh"].Value = p.ps.h.ToString();
//                  dgvPV.Rows[i-1].Cells["colPVer"].Value = p.ps.er.ToString();
//                  dgvPV.Rows[i-1].Cells["colPVbb"].Value= p.ps.bb.ToString();
//                  dgvPV.Rows[i-1].Cells["colPVso"].Value= p.ps.so.ToString();
//                  break;
//               case 1:
//                  dgvPH.Rows.Add();
//                  dgvPH.Rows[i-1].Cells["colPHname"].Value = p.pname;
//                  dgvPH.Rows[i-1].Cells["colPHip"].Value = CGame.StatDisplayStr(p.ps.ip3, StatCat.ip);
//                  dgvPH.Rows[i-1].Cells["colPHr"].Value = p.ps.r.ToString();
//                  dgvPH.Rows[i-1].Cells["colPHh"].Value = p.ps.h.ToString();
//                  dgvPH.Rows[i-1].Cells["colPHer"].Value = p.ps.er.ToString();
//                  dgvPH.Rows[i-1].Cells["colPHbb"].Value= p.ps.bb.ToString();
//                  dgvPH.Rows[i-1].Cells["colPHso"].Value= p.ps.so.ToString();
//                  break;
//            }   
//            i++;
//         }   
//         dgvPV.Refresh();
//         dgvPH.Refresh();
      }




      public async void UpdateBBox(int n, StatCat sc, int ab, int i) {
      // --------------------------------------------------------------

      // Update just the one cell...
         nint ix = mGame.t[ab].bat[i].bbox;

      // ixLim is thhe highest used elt in batters' box score.
      // Plus 1 equals the number of cells in boxscore (since we include total line).
         nint ixLim = mGame.BBoxLim(ab); 

         NSIndexPath[] rowsToReload = new NSIndexPath[] { 
         // Points to n-th row in the first section of the model...
         // and to the total row...
             NSIndexPath.FromRowSection(ix-1, 0),
             NSIndexPath.FromRowSection(ixLim, 0)
         };

         switch (ab) {
            case 0:
               tblBoxScoreV.ReloadRows(rowsToReload, UITableViewRowAnimation.Fade);
               //tblBoxScoreV.ReloadSections(x, UITableViewRowAnimation.Fade);
               tblBoxScoreH.ReloadData(); 
               break;
            case 1:
               tblBoxScoreH.ReloadRows(rowsToReload, UITableViewRowAnimation.Fade);
               //tblBoxScoreH.ReloadData(); //ReloadRows doesn't work!
               break;
         }

         await Task.Delay(100);


//      // n is the stat amount, i is the batter index.
//         int ix = mGame.bat[ab,i].bbox;
//         DataGridViewCell c = null;
//         DataGridView dgv = ab==0 ? dgvBV : dgvBH;
//         string pfx = ab==0 ? "colBV" : "colBH";
//
//         switch (sc) {
//            case StatCat.ab: c = dgv.Rows[ix-1].Cells[pfx+"ab"]; break;
//            case StatCat.r: c = dgv.Rows[ix-1].Cells[pfx+"r"]; break;
//            case StatCat.h: c = dgv.Rows[ix-1].Cells[pfx+"h"]; break;
//            case StatCat.rbi: c = dgv.Rows[ix-1].Cells[pfx+"rbi"]; break;
//            case StatCat.b2: c = dgv.Rows[ix-1].Cells[pfx+"b2"]; break;
//            case StatCat.b3: c = dgv.Rows[ix-1].Cells[pfx+"b3"]; break;
//            case StatCat.hr: c = dgv.Rows[ix-1].Cells[pfx+"hr"]; break;
//            case StatCat.bb: c = dgv.Rows[ix-1].Cells[pfx+"bb"]; break;
//            case StatCat.k: c = dgv.Rows[ix-1].Cells[pfx+"so"]; break;
//         };
//
//      // Highlight celland issue beep (#1506.02)
//         c.Style.ForeColor = Color.Blue;
//         c.Style.BackColor = Color.Yellow;
//         //c.Style.Font.Style = FontStyle.Bold;
//         //c.Style.Font.Bold = true;
//         //SystemSounds.Beep.Play();
//         if (c != null) c.Value = n.ToString();
//         dgv.Refresh();
//         if (!mGame.IsFastRunMode) {        
//            clicker.PlaySync();
//            //Console.Beep(400, 40); 
//            Thread.Sleep(150);
//         }
//         c.Style.ForeColor = Color.Black;
//         c.Style.BackColor = Color.White;
//         dgv.Refresh();
//         
      }


      public void UpdatePBox(int n, StatCat sc, int fl, int i) {
      // ------------------------------------------------

      // Update just the one cell...
            nint ix = mGame.t[fl].pit[i].pbox;

         // ixLim is the highest used elt in pitchers' box score.
         // ixLim is also the table view's index of the total row.
         // Plus 1 equals the number of cells in table view section (since we include total line).
            nint ixLim = mGame.PBoxLim(fl); 

            NSIndexPath[] rowsToReload = new NSIndexPath[] { 
            // Points to n-th row in the second section of the model...
            // and to the total row...
                NSIndexPath.FromRowSection(ix-1, 1),
                NSIndexPath.FromRowSection(ixLim, 1)
            };

            switch (fl) {
               case 0:
                  tblBoxScoreV.ReloadRows(rowsToReload, UITableViewRowAnimation.Fade);
                  break;
               case 1:
                  tblBoxScoreH.ReloadRows(rowsToReload, UITableViewRowAnimation.Fade);
                  break;
            }




//         int ix = mGame.pit[fl,i].pbox;
//         DataGridViewCell c = null;
//         DataGridView dgv = fl==0 ? dgvPV : dgvPH;
//         string pfx = fl==0 ? "colPV" : "colPH";
//
//         switch (sc) {
//            case StatCat.ip: c = dgv.Rows[ix-1].Cells[pfx+"ip"]; break;
//            case StatCat.h:  c = dgv.Rows[ix-1].Cells[pfx+"h"]; break;
//            case StatCat.r:  c = dgv.Rows[ix-1].Cells[pfx+"r"]; break;
//            case StatCat.er: c = dgv.Rows[ix-1].Cells[pfx+"er"]; break;
//            case StatCat.k:  c = dgv.Rows[ix-1].Cells[pfx+"so"]; break;
//            case StatCat.bb: c = dgv.Rows[ix-1].Cells[pfx+"bb"]; break;
//            case StatCat.hr: c = dgv.Rows[ix-1].Cells[pfx+"hr"]; break;
//         };
//         if (c != null) c.Value = CGame.StatDisplayStr(n, sc);
//
//      // Highlight celland issue beep (#1506.02)
//         c.Style.ForeColor = Color.Blue;
//         c.Style.BackColor = Color.Yellow;
//         dgv.Refresh(); 
//         //SystemSounds.Beep.Play();
//         if (!mGame.IsFastRunMode) { 
//            //Console.Beep(400, 40); 
//            clicker.PlaySync();
//            Thread.Sleep(150);
//         }
//
//         c.Style.ForeColor = Color.Black;
//         c.Style.BackColor = Color.White;
//         dgv.Refresh(); 

      }


   /* -------------------------------------------------------------------
    * The following methods deal with posting the linescore to the screen.
    * We always deal in actual game innings, not
    * adusted for any offset, but we apply the offset to determine the
    * physical linescore UILabel.
    * #1706.23
    * -------------------------------------------------------------------
    */

      int LinescoreStartInning = 1;


      public void InitLinescore() {
      // ---------------------------------------------------------
      // This just blanks out the whole linescore, and posts inning
      // numbers up to 9, or the current inning if more.
      // 1706.23
      
         for (int ab1= 0; ab1<=1; ab1++) {
            txtAbbrev[ab1].Text = mGame.t[ab1].lineName;
            for (int i=1; i<=12; i++) grdLinescore[ab1,i].Text = "";
         }
         for (int i = 1; i <= 12; i++) grdInning[i].Text = "";

         int iOff = LinescoreStartInning - 1;
         int iLim;
         if (mGame.inn <= 9) iLim = 9; else iLim = Math.Min(mGame.inn, LinescoreStartInning + 11);
         for (int i = LinescoreStartInning; i <= iLim; i++) grdInning[i-iOff].Text = i.ToString ();

      // Visibility of the 2 shift buttons...
         cmdShiftLeft.Hidden = mGame.inn <= 12;
         cmdShiftRight.Hidden = mGame.inn <= 12;
         
      }


      public void ShowLinescoreOne() {
         // ---------------------------------------------------------------
         // This just posts current inning (mGame.inn) for team at bat (mGame.ab).
         // If the inning is not in current range, it adjusts the display.
         // 1706.23

         if (mGame.inn >= LinescoreStartInning + 12) {
         // Inning is not in current range, must adjust...
            do 
               LinescoreStartInning += 12;
            while (mGame.inn >= LinescoreStartInning + 12) ;
            ShowLinescoreFull ();
         }              
         else {
         // Just post single half-inning score...  
            int iOff = LinescoreStartInning - 1;
            grdLinescore [mGame.ab, mGame.inn - iOff].Text = mGame.lines [mGame.ab, mGame.inn].ToString ();
            grdInning [mGame.inn - iOff].Text = mGame.inn.ToString ();
            ShowRHE ();
         }
         
      }


      public void ShowLinescoreFull() {
         // --------------------------------------------------------
         // This posts the entire linescore for game so far...
         // But just showing visible innings (based on LinescoreStartInning).
         // This is only used by the left- & right-shift buttons.
         // 1706.23

         int iLim;
         int iOff = LinescoreStartInning - 1;

      // Blank out existiing...
         InitLinescore ();

      // Visitors...
         iLim = Math.Min(mGame.inn, LinescoreStartInning + 11);
         for (int i = LinescoreStartInning; i <= iLim; i++) 
            grdLinescore[0, i-iOff].Text = mGame.lines[0,i].ToString();

         // Home...
         if (mGame.ab == 0) iLim = Math.Min(mGame.inn-1, LinescoreStartInning + 11);
         for (int i = LinescoreStartInning; i<=iLim; i++) {
            grdLinescore[1, i-iOff].Text= mGame.lines[1,i].ToString();
         }

         ShowRHE ();


      }



      private void EnableControls() {
      // ----------------------------------------------------------------
         switch (mGame.PlayState) {
            case PLAY_STATE.PLAY: 
              cmdManageV.Enabled = cmdManageH.Enabled = true;
              ///cmdGo.ForeColor = Color.SlateGray; 
              cmdGo.Enabled = cmdInfo.Enabled = true;  
            //cmdGo.TitleLabel.Text = "Play"; ///cmdGo.Select(); 
              cmdGo.SetTitle("Play", UIControlState.Normal);
//              toolTip2.Active = true;
//              toolTip2.SetToolTip(cmdGo, "Click (or press space bar) to start the at-bat");
//              lblStatus.Text = "Click 'Play' (or press space bar) to start the at-bat";
              break;
            case PLAY_STATE.NEXT: 
              cmdManageV.Enabled = cmdManageH.Enabled = false;
              ///cmdGo.ForeColor = Color.SlateGray;
              cmdGo.Enabled = cmdInfo.Enabled = true;
            //cmdGo.TitleLabel.Text = "Next"; ///cmdGo.Select();
              cmdGo.SetTitle("Next", UIControlState.Normal);
//              toolTip2.Active = true;
//              toolTip2.SetToolTip(cmdGo, "Click (or press space bar) to bring up the next batter");
//              lblStatus.Text = "Click 'Next' (or press space bar) to bring up the next batter";
              break;
            case PLAY_STATE.START: 
              cmdManageV.Enabled = cmdManageH.Enabled = true;
               ///cmdGo.ForeColor = Color.SlateGray;
               cmdGo.Enabled = true;
               cmdInfo.Enabled = false;
            //cmdGo.TitleLabel.Text = " Start"; ///cmdGo.Select(); 
              cmdGo.SetTitle("Start", UIControlState.Normal);
//              toolTip2.Active = true;
//              toolTip2.SetToolTip(cmdGo, "Click (or press space bar) to bring up the first batter");
//              lblStatus.Text = "Click 'Start' (or press space bar) to bring up the first batter";
              break;
            case PLAY_STATE.NONE: 
              cmdManageV.Enabled = cmdManageH.Enabled = false;
              ///cmdGo.ForeColor = Color.LightGray;
              cmdGo.Enabled = cmdInfo.Enabled = false;
            //cmdGo.TitleLabel.Text = "";  
              cmdGo.SetTitle("", UIControlState.Normal);
//              toolTip2.Active = false;
//              lblStatus.Text = "Click 'New Game' to select teams and start a new game";
              break;
         }      
            
      }
     

//		partial void cmdListVoices_TouchUpInside (UIButton sender)
//		{
//			throw new NotImplementedException ();
//		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}


      private void ShowAlert(string title, string msg, UIViewController ctlr) {
         // -------------------------------------------------------------------
         UIAlertController alertController; 
         string titleUse = title == "" ? "Error" : title;
         alertController = UIAlertController.Create(titleUse, msg, UIAlertControllerStyle.Alert);
         alertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, alert => {
            // Do nothing...
         }));

         ctlr.PresentViewController(alertController, true, null);

      }

      //partial void CmdManageV_TouchUpInside(UIButton sender)
      //{
      //   throw new NotImplementedException();
      //}
   }






   }