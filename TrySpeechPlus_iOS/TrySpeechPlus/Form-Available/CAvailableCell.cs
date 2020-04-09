using System;
using Foundation;
using UIKit;

namespace BCX.BCXB
{
   public class CAvailableCell : UITableViewCell {

   // Members
   // -------
      UILabel lblName, lblSkill;



   // Constructor:
      public CAvailableCell(Foundation.NSString cellId) : base (UITableViewCellStyle.Default, cellId) {
      // ---------------------------------------------------------------------

         SelectionStyle = UITableViewCellSelectionStyle.Gray;
         //ContentView.BackgroundColor = UIColor.White;
         nfloat fontSize = 18f;

         lblName = new UILabel () {
            Font = UIFont.FromName("Arial", fontSize), 
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Left,
            //BackgroundColor = UIColor.White
         };
         lblSkill = new UILabel () {
            Font = UIFont.FromName("Arial", fontSize), 
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Left,
            //BackgroundColor = UIColor.White
         };

         ContentView.AddSubviews(new UIView[] {
            lblName, lblSkill}); 
      }


      public void UpdateCell(CGame gm1, int ab, CLineupCard lu1, NSIndexPath ixp) {
      // --------------------------------------------------
         //int bx = gm1.bat[ab, ixp.Row+1].bx;
         if (ixp.Row > lu1.Available.Count - 1) return;
         int bx = lu1.Available[ixp.Row].bx;
         CBatter bat1 = gm1.t[ab].bat[bx]; 

         lblName.Text = bat1.DisplayName;
         lblSkill.Text = bat1.DisplaySkill;

      }

      public override void LayoutSubviews() {
      // ------------------------------------------------
         base.LayoutSubviews ();
         lblName.Frame = new CoreGraphics.CGRect (0, 0, 110, 30);  
         lblSkill.Frame = new CoreGraphics.CGRect (110,0,150,30); 

      }

   }
}

