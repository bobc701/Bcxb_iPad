using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Collections.Generic;
using BCX.BCXCommon;


namespace BCX.BCXB
{

   // ------------------------------------------------------------
   partial class PickTeamsController : UIViewController
   {
      // ------------------------------------------------------------

      // Input: Caller provides empty selectedTeams array (in Prep4Seg).
      // Output: selectedTeams filled with 2 (V & H) CTeamRecord's.

      public CTeamRecord [] selectedTeams; //This is supplied by the caller & is returned filled.
      private List<string> yearList;
      private CTeamRecord selectedTeam; //Need just 1 of these.


      public PickTeamsController (IntPtr handle) : base (handle) {
         // ---------------------------------------------------------
         //leagueList = new List<string> { "NL2014", "AL2014", "NL2015", "AL2015" };
         //leagueList = GFileAccess.GetLeagueList ();
         yearList = GFileAccess.GetYearList();

      }


      public override void ViewDidLoad () {
      // ----------------------------------
         base.ViewDidLoad ();
         // Perform any additional setup after loading the view, typically from a nib.
         Activity1.Hidden = true;
         SetupPicker();
      
      }


      public override void ViewDidDisappear(bool animated) {
      // ---------------------------------------------------------
         base.ViewDidDisappear(animated);
         GFileAccess.ClearTeamCache();

      }


      public void StartActivity() {
      // --------------------------------
         Activity1.Hidden = false;
         Activity1.StartAnimating();
      }


      public void StopActivity() {
      // -------------------------------
         Activity1.StopAnimating();
         Activity1.Hidden = true;

      }


      public void SetupPicker ()
      {
         // ----------------------

         // Set up picker and model...
         // Note: We need separate models for V & H because need to update
         // separate TextField's...
         var modelBottomV = new CPickerModel (yearList, this);
         var modelBottomH = new CPickerModel (yearList, this);

         // Instantiate the events with handlers...
         modelBottomV.PickerChanged += delegate (CTeamRecord t, bool valid) {
            selectedTeam = t;
            selectedTeams[0] = t;
            txtTeamV.Text = valid ? $"{t.Year} {t.LineName} {t.NickName}" : "";
            cmdDone.Enabled = (txtTeamV.Text != "" && txtTeamH.Text != "");
         };

         modelBottomH.PickerChanged += delegate (CTeamRecord t, bool valid) {
            selectedTeam = t;
            selectedTeams[1] = t;
            txtTeamH.Text = valid ? $"{t.Year} {t.LineName} {t.NickName}" : "";
            cmdDone.Enabled = (txtTeamV.Text != "" && txtTeamH.Text != "");
         };

         // Setup 2 picker views...
         UIPickerView pickerBottomV = new UIPickerView ();
         pickerBottomV.ShowSelectionIndicator = true;
         pickerBottomV.Model = modelBottomV;
         UIPickerView pickerBottomH = new UIPickerView ();
         pickerBottomH.ShowSelectionIndicator = true;
         pickerBottomH.Model = modelBottomH;

         // Set up 2 toolbars...
         var tbV = new UIToolbar ();
         tbV.BarStyle = UIBarStyle.Black;
         tbV.Translucent = true;
         tbV.SizeToFit ();

         var tbH = new UIToolbar ();
         tbH.BarStyle = UIBarStyle.Black;
         tbH.Translucent = true;
         tbH.SizeToFit ();


         // Create 2 'Done' buttons and add them to respective toobar...
         var cmdDoneV = new UIBarButtonItem ("Done", UIBarButtonItemStyle.Done,
            (s, e) => {
               this.txtTeamV.Text = $"{selectedTeam.Year} {selectedTeam.LineName} {selectedTeam.NickName}";
               this.selectedTeams[0] = selectedTeam;
               this.txtTeamV.ResignFirstResponder ();
            });
         tbV.SetItems (new UIBarButtonItem [] { cmdDoneV }, true);

         var cmdDoneH = new UIBarButtonItem ("Done", UIBarButtonItemStyle.Done,
            (s, e) => {
               this.txtTeamH.Text = $"{selectedTeam.Year} {selectedTeam.LineName} {selectedTeam.NickName}";
               this.selectedTeams[1] = selectedTeam;
               this.txtTeamH.ResignFirstResponder ();
            });
         tbH.SetItems (new UIBarButtonItem [] { cmdDoneH }, true);

         // Tell the textbox's to use the pickers for input & display toolbar 
         // over it...
         this.txtTeamV.InputView = pickerBottomV;
         this.txtTeamV.InputAccessoryView = tbV;
         this.txtTeamH.InputView = pickerBottomH;
         this.txtTeamH.InputAccessoryView = tbH;
         
      }



   }
         
   // --------------------------------------------------
      public class CPickerModel : UIPickerViewModel {
   // ---------------------------------------------------

         private bool usingDh;
         List<string> yearList;
         List<CTeamRecord> teamList;
         internal PickTeamsController ctlr;

         public event Action<CTeamRecord, bool> PickerChanged;

         // 
         // Constructors
         //

         internal CPickerModel (List<string> yearList1, PickTeamsController ctlr1) {
         // --------------------------------------------

            yearList = yearList1;
         ///teamList = GFileAccess.GetTeamsInLeague(leagueList[0], out usingDh); //, out usingDh);
           
         // We start with an empty team list, user must pick a year first...
         // Later, in the Selected method, for component 0 (year), 
         // GetTeamsInYear will be called to generate an actual team list.
            teamList = new List<CTeamRecord>();
            ctlr = ctlr1;
         }

         public CPickerModel () {

         }


         public override nint GetComponentCount (UIPickerView picker) {
         // ---------------------------------------------------------
            return 2;
         }

         public override nint GetRowsInComponent (UIPickerView picker, nint component) {
         // --------------------------------------------------------------------------
            switch (component) {
               case 0: return yearList.Count+1; 
               case 1: return teamList.Count+1;
               default: return 0;
            }
         }

         public override string GetTitle (UIPickerView picker, nint row, nint component) {
         // ----------------------------------------------------------------------------
            switch (component) {  
               case 0:
                  if (row == 0) return "Choose year";
                  else return yearList[(int)row-1];   
               case 1:
                  if (row == 0) return "Choose team";
                  else {
                     CTeamRecord t = teamList[(int)row - 1];
                     return $"{t.LgID} - {t.City} {t.NickName}";
                  }
               default: return null;   
            }  

         }

         public async override void Selected (UIPickerView picker, nint row, nint component) {
         // ---------------------------------------------------------------------------
            try {
               ctlr.StartActivity();
               switch (component) {
                  case 0:
                     if (row == 0)
                        teamList.Clear();
                     else {
                        string yr = yearList[(int)row - 1];
                        //teamList = GFileAccess.GetTeamsInLeague(s, out usingDh);
                        teamList = await GFileAccess.GetTeamListForYearFromCache(int.Parse(yr));
                     } 
                     picker.Select(row: 0, component: 1, false); // Reset team to row 0
                     picker.ReloadComponent(1);
                     break;
                  case 1:
                     if (row == 0)
                        PickerChanged(new CTeamRecord(), false);
                     else
                        PickerChanged(teamList[(int)row - 1], true);
                     break;
               }
            ctlr.StopActivity();
            }
            catch (Exception ex) {
            ctlr.StopActivity();
               CAlert.ShowOkAlert("Error selecting year", ex.Message, "OK", ctlr);
            }
         }

      }

   }

