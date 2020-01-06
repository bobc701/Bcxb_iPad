using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using CoreGraphics;


namespace BCX.BCXB
{
	partial class TryDrawController : UIViewController
	{

      private GProfileDisk draw1, draw2, draw3, draw4;
      public CGame g; //Assigned in PrepareForSegue.
      CBatter b;
      CPitcher p;

      public TryDrawController (IntPtr handle) : base (handle) {
      // -------------------------------------------------------
         //draw1 = new CDrawHandler ();

//         int i = g.linup[g.ab, g.slot[g.ab]];
//         int j = g.curp[g.fl];
//         b = g.bat[g.ab, i];
//         p = g.pit[g.ab, j];

      }


      public void AssignGame(CGame g1) {
         // --------------------------------
         g = g1;
         int i = g.t[g.ab].linup[g.t[g.ab].slot];
         int j = g.t[g.fl].curp;
         b = g.t[g.ab].bat[i];
         p = g.t[g.fl].pit[j];
      }


      public override void ViewDidLoad () {
      // ----------------------------------
         base.ViewDidLoad ();
         // Perform any additional setup after loading the view, typically from a nib.


      // Get name of batter & pitcher for labels on the disks...
         string sBatter = b.bname;
         string sPitcher = p.pname;

         CoreGraphics.CGRect x = UIScreen.MainScreen.Bounds;

         //draw1 = new GProfileDisk (200, 300, g.cmean){Frame = UIScreen.MainScreen.Bounds};
         draw1 = new GProfileDisk (150, 200, g.cmean){Frame = new CGRect(40D, 80D, 300D,400D)};
         draw1.DiceRoll = g.diceRollBatting;
         draw1.ProfileLabel = "League Norm:";
         draw1.Opaque = false;
         View.AddSubview (draw1);

         //draw2 = new GProfileDisk (550, 300, b.par){Frame = UIScreen.MainScreen.Bounds};
         draw2 = new GProfileDisk (150, 200, b.par){Frame = new CGRect(360D, 80D, 300D, 400D)};
         draw2.DiceRoll = g.diceRollBatting;
         draw2.ProfileLabel = sBatter + " vs. League Norm:";
         draw2.SubLabel1 = BatterStatsString(1);
         draw2.SubLabel2 = BatterStatsString(2);
         draw2.Opaque = false;
         View.AddSubview (draw2);

         //draw3 = new GProfileDisk (200, 650, p.par){Frame = UIScreen.MainScreen.Bounds};
         draw3 = new GProfileDisk (150, 200, p.par){Frame = new CGRect(40D, 420D, 300D, 400D)};  
         draw3.DiceRoll = g.diceRollBatting;
         draw3.ProfileLabel = sPitcher + " vs. League Norm:";
         draw3.SubLabel1 = PitcherStatsString(1);
         draw3.SubLabel2 = PitcherStatsString(2);
         draw3.Opaque = false;
         View.AddSubview (draw3);

         //draw4 = new GProfileDisk (550, 650, g.cpara){Frame = UIScreen.MainScreen.Bounds};
         draw4 = new GProfileDisk (150, 200, g.cpara){Frame = new CGRect(360D, 420D, 300D, 400D)};
         draw4.DiceRoll = g.diceRollBatting;
         draw4.ProfileLabel = sBatter + " vs. " + sPitcher + ":";
         draw4.Opaque = false;
         View.AddSubview (draw4);


      }

      private string BatterStatsString(int i) {
      // ----------------------------------------------
         string s = "";
         switch (i) {
            case 1:
               s = string.Format(
                  "ab:{0}, ave:{1:#.000}, hr:{2}, 3b:{3}, 2b:{4}", 
                  b.br.ab, b.br.ave, b.br.hr, b.br.b3, b.br.b2);
               break;
            case 2:
               s = string.Format(
                  "rbi:{0}, bb:{1}, so{2}", 
                  b.br.bi, b.br.bb, b.br.so);
               break;
         }
         return s;

      }


      private string PitcherStatsString(int i) {
      // ----------------------------------------------
         string s = "";
         CPitRealSet r = p.pr;
         switch (i) {
            case 1:
               double whip = r.ip3>0 ? (r.h+r.bb) / (r.ip3/3.0) : 0.0;
               s = string.Format(
                  "ip:{0:#0.0}, era:{1:#0.00}, whip:{2:#0.00}", 
                  r.ip3/3.0, r.era, whip);
               break;
            case 2:
               s = string.Format(
                  "h:{0}, hr:{1}, so:{2}, bb:{3}", 
                  r.h, r.hr, r.so, r.bb);
               break;
         }
         return s;

      }
   

	}
}
