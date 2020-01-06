using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace BCX.BCXB
{
	partial class SpecialPlaysController : UIViewController
	{
      public enum EGameState {PreGame, Offense, Defense};

      public CGame g;
      public SPECIAL_PLAY Play = SPECIAL_PLAY.AtBat;

   // Contructor:
		public SpecialPlaysController (IntPtr handle) : base (handle)
		{
		}


      public override void ViewDidLoad() {
         // -----------------------------------
         base.ViewDidLoad();

         if (g.PlayState == PLAY_STATE.START || g.PlayState == PLAY_STATE.NONE) {
            EnableButton(cmdSac, false);  
            EnableButton(cmdSteal, false);
            EnableButton(cmdIP, false); 
         } else {   
            EnableButton(cmdSac, true);
            EnableButton(cmdSteal, true);
            EnableButton(cmdIP, true);         
         }

         Play = g.specialPlay;
         switch (g.specialPlay) {
            case SPECIAL_PLAY.Bunt: 
               cmdSac.On = true;
               cmdSteal.On = false;
               cmdIP.On = false; 
               break;
            case SPECIAL_PLAY.Steal:  
               cmdSac.On = false; 
               cmdSteal.On = true;
               cmdIP.On = false; 
               break;
            case SPECIAL_PLAY.IP: 
               cmdSac.On = false;
               cmdSteal.On = false;
               cmdIP.On = true;  
               break;
            default:
               cmdSac.On = false;
               cmdSteal.On = false;
               cmdIP.On = false;
               break;
         }


         cmdSac.ValueChanged += delegate(object sender, EventArgs e) {
         // ----------------------------------------------------------
            if (cmdSac.On) {
               cmdSteal.On = false;
               cmdIP.On = false;
               Play = SPECIAL_PLAY.Bunt;
            }
            else Play = SPECIAL_PLAY.AtBat;

         };

         cmdSteal.ValueChanged += delegate(object sender, EventArgs e) {
         // ----------------------------------------------------------
            if (cmdSteal.On) {
               cmdSac.On = false;
               cmdIP.On = false;
               Play = SPECIAL_PLAY.Steal;
            }
            else Play = SPECIAL_PLAY.AtBat;

         };

         cmdIP.ValueChanged += delegate(object sender, EventArgs e) {
         // ----------------------------------------------------------
            if (cmdIP.On) {
               cmdSteal.On = false;
               cmdSac.On = false;
               Play = SPECIAL_PLAY.IP;
            }
            else Play = SPECIAL_PLAY.AtBat;
         };


      }

      private void EnableButton (UISwitch btn, bool on) { 
      // ---------------------------------------------------------------
         if (on) { 
            btn.Enabled = true; 
            //btn.TitleColor(UIControlState.Normal) = UIColor.Blue;
         } 
         else {
            btn.Enabled = false;
            //btn.TitleColor(UIControlState.Disabled) = UIColor.DarkGray;
         }       
      } 
       
	}

}


