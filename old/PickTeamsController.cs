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
      private List<string> leagueList;
      private CTeamRecord selectedTeam; //Need just 1 of these.


      public PickTeamsController (IntPtr handle) : base (handle)
      {
         // ---------------------------------------------------------
         //leagueList = new List<string> { "NL2014", "AL2014", "NL2015", "AL2015" };
         leagueList = GFileAccess.GetLeagueList();
      }


      public override void ViewDidLoad ()
      {
         // ----------------------------------
         base.ViewDidLoad ();
         // Perform any additional setup after loading the view, typically from a nib.

         SetupPicker ();

      }


      public void SetupPicker ()
      {
         // ----------------------

         // Set up picker and model...
         // Note: We need separate models for V & H because need to update
         // separate TextField's...
         var modelBottomV = new CPickerModel (leagueList);
         var modelBottomH = new CPickerModel (leagueList);

         // Instantiate the events with handlers...
         modelBottomV.PickerChanged += delegate (CTeamRecord t, bool valid) {
            selectedTeam = t;
            selectedTeams[0] = t;
            txtTeamV.Text = valid ? t.City : "";
            cmdDone.Enabled = (txtTeamV.Text != "" && txtTeamH.Text != "");
         };

         modelBottomH.PickerChanged += delegate (CTeamRecord t, bool valid) {
            selectedTeam = t;
            selectedTeams[1] = t;
            txtTeamH.Text = valid ? t.City : "";
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
               this.txtTeamV.Text = selectedTeam.City;
               this.selectedTeams[0] = selectedTeam;
               this.txtTeamV.ResignFirstResponder ();
            });
         tbV.SetItems (new UIBarButtonItem [] { cmdDoneV }, true);

         var cmdDoneH = new UIBarButtonItem ("Done", UIBarButtonItemStyle.Done,
            (s, e) => {
               this.txtTeamH.Text = selectedTeam.City;
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
         List<string> leagueList;
         List<CTeamRecord> teamList;

         public event Action<CTeamRecord, bool> PickerChanged;

         // 
         // Constructors
         //

         public CPickerModel (List<string> leagueList1) {
         // --------------------------------------------

            leagueList = leagueList1;
         ///teamList = GFileAccess.GetTeamsInLeague(leagueList[0], out usingDh); //, out usingDh);
           
         // We start with an empty team list, user must pick a league first...
         // Later, in the Selected method, for component 0 (league), 
         // GetTeamsInLeague will be called to generate an actual team list.
            teamList = new List<CTeamRecord>();
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
               case 0: return leagueList.Count+1; 
               case 1: return teamList.Count+1;
               default: return 0;
            }
         }

         public override string GetTitle (UIPickerView picker, nint row, nint component) {
         // ----------------------------------------------------------------------------
            switch (component) {  
               case 0:
                  if (row == 0) return "Choose league";
                  else return leagueList[(int)row-1];   
               case 1:
                  if (row == 0) return "Choose team";
                  else return teamList[(int)row-1].City;
               default: return null;   
            }  

         }

         public override void Selected (UIPickerView picker, nint row, nint component) {
         // ---------------------------------------------------------------------------
            switch (component) {
               case 0:
                  if (row == 0)
                     teamList.Clear ();
                  else {
                     string s = leagueList[(int)row - 1];
                     teamList = GFileAccess.GetTeamsInLeague (s, out usingDh);
                  }  
                  picker.ReloadComponent(1);
                  break;  
               case 1:
                  if (row == 0) 
                     PickerChanged (new CTeamRecord (), false);
                  else
                     PickerChanged(teamList[(int)row-1], true);
                  break;
            }
         }

      }

   }

