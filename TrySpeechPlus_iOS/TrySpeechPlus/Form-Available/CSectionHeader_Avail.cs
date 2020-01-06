using System;
using UIKit;
using Foundation;

namespace BCX.BCXB
{
   public class CSectionHeader_Avail : UITableViewHeaderFooterView {
   // ===========================================================

      UILabel lblTitle, lblSkill; 

      public CSectionHeader_Avail(NSString cellid) : base(cellid) { 

         //SelectionStyle = UITableViewCellSelectionStyle.Gray;
         ContentView.BackgroundColor = UIColor.LightGray;
         string fontName = "Arial";
         nfloat fontSize = 15f;

         lblTitle = new UILabel () {
            Font = UIFont.FromName(fontName, fontSize),
            TextColor = UIColor.Blue,
            TextAlignment = UITextAlignment.Left,
            BackgroundColor = UIColor.White,
            Text = "Player"
         };
         lblSkill = new UILabel () { 
            Font = UIFont.FromName(fontName, fontSize),
            TextColor = UIColor.Blue,
            TextAlignment = UITextAlignment.Left,
            BackgroundColor = UIColor.White,
            Text = "Fielding"
         }; 


         ContentView.AddSubviews(new UIView[] {
            lblTitle, lblSkill}); 


      }

      public override void LayoutSubviews() {
      // -------------------------------------
         base.LayoutSubviews(); 

         lblTitle.Frame = new CoreGraphics.CGRect (0, 0, 120, 40);  
         lblSkill.Frame = new CoreGraphics.CGRect(120, 0, 220, 40);  

      }

   }

}

