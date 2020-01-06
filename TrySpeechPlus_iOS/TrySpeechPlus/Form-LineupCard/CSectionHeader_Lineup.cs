using System;
using UIKit;
using Foundation;

namespace BCX.BCXB
{
   public class CSectionHeader_Lineup : UITableViewHeaderFooterView {
   // ===========================================================

      UILabel lblSlot, lblTitle, lblPos, lblBase, lblSkill; 

      public CSectionHeader_Lineup(NSString cellid) : base(cellid) { 

         //SelectionStyle = UITableViewCellSelectionStyle.Gray;
         ContentView.BackgroundColor = UIColor.LightGray;
         string fontName = "Arial";
         nfloat fontSize = 15f;

         lblSlot = new UILabel () {
            Font = UIFont.FromName(fontName, fontSize),
            TextColor = UIColor.Blue,
            TextAlignment = UITextAlignment.Center, 
            BackgroundColor = UIColor.White,
            Text = ""
         };
         lblTitle = new UILabel () {
            Font = UIFont.FromName(fontName, fontSize),
            TextColor = UIColor.Blue,
            TextAlignment = UITextAlignment.Left,
            BackgroundColor = UIColor.White,
            Text = "Player"
         };
         lblPos = new UILabel () { 
            Font = UIFont.FromName(fontName, fontSize),
            TextColor = UIColor.Blue,
            TextAlignment = UITextAlignment.Center,
            BackgroundColor = UIColor.White,
            Text = "Pos"
         };

         lblBase = new UILabel () {
            Font = UIFont.FromName(fontName, fontSize),
            TextColor = UIColor.Blue,
            TextAlignment = UITextAlignment.Center,
            BackgroundColor = UIColor.White,
            Text = "Base"
         };
         lblSkill = new UILabel () { 
            Font = UIFont.FromName(fontName, fontSize),
            TextColor = UIColor.Blue,
            TextAlignment = UITextAlignment.Left,
            BackgroundColor = UIColor.White,
            Text = "Fielding"
         }; 


         ContentView.AddSubviews(new UIView[] {
            lblSlot, lblTitle, lblPos, lblBase, lblSkill}); 


      }

      public override void LayoutSubviews() {
      // -------------------------------------
         base.LayoutSubviews(); 

         lblSlot.Frame = new CoreGraphics.CGRect (0, 0, 30, 40);
         lblTitle.Frame = new CoreGraphics.CGRect (30, 0, 110, 40);  
         lblPos.Frame = new CoreGraphics.CGRect (140,0,30,40); 
         lblBase.Frame = new CoreGraphics.CGRect (170,0,50,40); 
         lblSkill.Frame = new CoreGraphics.CGRect(220, 0, 150, 40);  


      }

   }

}

