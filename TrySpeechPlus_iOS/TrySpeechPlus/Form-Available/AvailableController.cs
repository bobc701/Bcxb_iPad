using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace BCX.BCXB
{
	public partial class AvailableController : UIViewController
	{
      public CGame g;
      public side abMng; //The side (home or vis) being managed here.
      public side abGame;    //The side currently at bat in the game
      public CLineupCard lineupCard;
      public AvailableController fAvail; 
      public CLineupChange operation;

      public int SelectedRow { get; set; }
      public CBatter SelectedBatter { get; set; }

   // Constructor:
      public AvailableController (IntPtr handle) : base (handle) {
         // ---------------------------------------------------------
         SelectedRow = -1;
      }
     

//      public void RowSelected(int r, CBatter bat1) {
//   // ------------------------------------------------------------------------
//   // This is intended to be called by CAavailableSource object from that
//   // class's RowSelected mothod...
//         this.SelectedRow = r;
//         this.SelectedBatter = bat1;
//
//         f
//      }

    

      public override void ViewDidLoad () {
      // ----------------------------------
         base.ViewDidLoad ();
      // Perform any additional setup after loading the view, typically from a nib.

         abGame = (side)g.ab;
         dgvAvailable.Source = new CAvailableSource((int)abMng, this);
         SetButtonColors(cmdDone);

         string msg = g.t[(int)abMng].nick + " Available:";
         lblHeader.Text = msg; 
         
         
      }

   

      private void SetButtonColors(UIButton btn) {
   // -------------------------------------------
      btn.BackgroundColor = UIColor.LightGray;
      btn.SetTitleColor(UIColor.Blue, UIControlState.Normal);
      btn.SetTitleColor(UIColor.Gray, UIControlState.Disabled);

   }

	}
}
