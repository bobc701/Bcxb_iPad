using System;
using Foundation;
using UIKit;

namespace BCX.BCXB {


   public class CLineupCardCell : UITableViewCell {
  
   // Members
   // -------
      UILabel lblName, lblSlot, lblPos, lblBase, lblSkill;



   // Constructor:
      public CLineupCardCell(Foundation.NSString cellId) : base (UITableViewCellStyle.Default, cellId) {
      // ---------------------------------------------------------------------

         SelectionStyle = UITableViewCellSelectionStyle.Gray;
         //ContentView.BackgroundColor = UIColor.White;

         string fontName = "Arial";
         nfloat fontSize = 15f;

         lblName = new UILabel () {
            Font = UIFont.FromName(fontName, fontSize), 
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Left
            //BackgroundColor = UIColor.White
         };
         lblSlot = new UILabel () {
            Font = UIFont.FromName(fontName, fontSize),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Center,
            BackgroundColor = UIColor.White
         };
         lblPos = new UILabel () { 
            Font = UIFont.FromName(fontName, fontSize),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Center
            //BackgroundColor = UIColor.White
         };
         lblBase = new UILabel () {
            Font = UIFont.FromName(fontName, fontSize),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Center
            //BackgroundColor = UIColor.White
         };
         lblSkill = new UILabel () {
            Font = UIFont.FromName(fontName, fontSize),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Left
            //BackgroundColor = UIColor.White
         };

         ContentView.AddSubviews(new UIView[] {
            lblName, lblSlot, lblPos, lblBase, lblSkill}); 
      }


      public void UpdateCell(CGame gm1, int ab, CLineupCard lu1, NSIndexPath ixp) {
      // --------------------------------------------------
         //int bx = gm1.bat[ab, ixp.Row+1].bx;
         int bx = lu1.CurrentLineup[ixp.Row].bx;
         CBatter bat1 = gm1.t[ab].bat[bx]; 


         lblSlot.Text = bat1.DisplaySlot;
         lblName.Text = bat1.DisplayName;
         lblPos.Text = bat1.DisplayPos;
         lblBase.Text = bat1.DisplayBase;
         lblSkill.Text = bat1.DisplaySkill;

      }

      public override void LayoutSubviews() {
      // ------------------------------------------------
         base.LayoutSubviews ();
         lblSlot.Frame = new CoreGraphics.CGRect (0, 0, 30, 30);
         lblName.Frame = new CoreGraphics.CGRect (30, 0, 110, 30);  
         lblPos.Frame = new CoreGraphics.CGRect (140,0,30,30); 
         lblBase.Frame = new CoreGraphics.CGRect (170,0,50,30); 
         lblSkill.Frame = new CoreGraphics.CGRect(220, 0, 150, 30);

      }

   }

}

