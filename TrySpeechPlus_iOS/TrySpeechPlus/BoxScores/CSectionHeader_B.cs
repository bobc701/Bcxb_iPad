using System;
using UIKit;
using Foundation;

namespace BCX.BCXB
{
   public class CSectionHeader_B : UITableViewHeaderFooterView {
   // ===========================================================

      UILabel lblTitle, lblAb, lblR, lblH, lblRbi, lblB2, lblB3, lblHr, lblBb, lblSo;

      public CSectionHeader_B(NSString cellid) : base(cellid) {

         //SelectionStyle = UITableViewCellSelectionStyle.Gray;
         ContentView.BackgroundColor = UIColor.LightGray;
         string fontName = "AmericanTypewriter";

         lblTitle = new UILabel () {
            Font = UIFont.FromName("Arial", 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Left,
            BackgroundColor = UIColor.White,
            Text = "Batting:"
         }; //x
         lblAb = new UILabel () {
            Font = UIFont.FromName(fontName, 12f), 
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White,
            Text = "ab"
         };
         lblR = new UILabel () { 
            Font = UIFont.FromName(fontName, 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White,
            Text = "r"
         };

         lblH = new UILabel () {
            Font = UIFont.FromName(fontName, 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White,
            Text = "h"
         };
         lblRbi = new UILabel () { 
            Font = UIFont.FromName(fontName, 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White,
            Text = "rbi"
         }; 
         lblB2 = new UILabel () { 
            Font = UIFont.FromName(fontName, 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White,
            Text = "2b"
         }; 
         lblB3 = new UILabel () { 
            Font = UIFont.FromName(fontName, 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White,
            Text = "3b"
         }; 
         lblB3 = new UILabel () { 
            Font = UIFont.FromName(fontName, 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White,
            Text = "3b"
         }; 
         lblHr = new UILabel () { 
            Font = UIFont.FromName(fontName, 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White,
            Text = "hr"
         }; 
         lblBb = new UILabel () { 
            Font = UIFont.FromName(fontName, 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White,
            Text = "bb"
         }; 
         lblSo = new UILabel () { 
            Font = UIFont.FromName(fontName, 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White,
            Text = "so"
         }; 

         ContentView.AddSubviews(new UIView[] {
            lblTitle, lblAb, lblR, lblH, lblRbi, lblB2, lblB3, lblHr, lblBb, lblSo});


      }

      public override void LayoutSubviews() {
      // -------------------------------------
         base.LayoutSubviews(); 
         lblTitle.Frame = new CoreGraphics.CGRect (0, 0, 70, 12); 
         lblAb.Frame = new CoreGraphics.CGRect (70, 0, 26, 12);
         lblR.Frame = new CoreGraphics.CGRect (96,0,26,12);
         lblH.Frame = new CoreGraphics.CGRect (122,0,26,12);
         lblRbi.Frame = new CoreGraphics.CGRect (148,0,26,12);
         lblB2.Frame = new CoreGraphics.CGRect (174,0,26,12);
         lblB3.Frame = new CoreGraphics.CGRect (200,0,26,12);
         lblHr.Frame = new CoreGraphics.CGRect (226,0,26,12);
         lblBb.Frame = new CoreGraphics.CGRect (252,0,26,12);
         lblSo.Frame = new CoreGraphics.CGRect (278,0,26,12);

      }

   }


   public class CSectionHeader_P : UITableViewHeaderFooterView {
   // ==========================================================

      UILabel lblTitle, lblIp, lblR, lblH, lblEr, lblBb, lblSo, lblHr;


      public CSectionHeader_P(NSString cellid) : base(cellid) {

         //SelectionStyle = UITableViewCellSelectionStyle.Gray;
         ContentView.BackgroundColor = UIColor.LightGray;
         string fontName = "AmericanTypewriter";

         lblTitle = new UILabel () {
            Font = UIFont.FromName("Arial", 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Left,
            BackgroundColor = UIColor.White,
            Text = "Pitching:"
         };
         lblIp = new UILabel () {
            Font = UIFont.FromName(fontName, 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White,
            Text = "ip"
         };
         lblR = new UILabel () { 
            Font = UIFont.FromName(fontName, 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White,
            Text = "r"
         };

         lblH = new UILabel () {
            Font = UIFont.FromName(fontName, 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White,
            Text = "h"
         };
         lblEr = new UILabel () { 
            Font = UIFont.FromName(fontName, 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White,
            Text = "er"
         }; 
         lblBb = new UILabel () { 
            Font = UIFont.FromName(fontName, 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White,
            Text = "bb"
         }; 
         lblSo = new UILabel () { 
            Font = UIFont.FromName(fontName, 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White,
            Text = "so"
         }; 
         lblHr = new UILabel () { 
            Font = UIFont.FromName(fontName, 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White,
            Text = "hr"
         }; 

         ContentView.AddSubviews(new UIView[] {lblTitle, lblIp, lblR, lblH, lblEr, lblBb, lblSo, lblHr}); 


      }

      public override void LayoutSubviews() {
      // -------------------------------------
         base.LayoutSubviews(); 
         lblTitle.Frame = new CoreGraphics.CGRect (0, 0, 70, 12); 
         lblIp.Frame = new CoreGraphics.CGRect (70, 0, 26, 12);
         lblR.Frame = new CoreGraphics.CGRect (96,0,35,12);
         lblH.Frame = new CoreGraphics.CGRect (131,0,35,12);
         lblEr.Frame = new CoreGraphics.CGRect (166,0,35,12);
         lblBb.Frame = new CoreGraphics.CGRect (201,0,35,12);
         lblSo.Frame = new CoreGraphics.CGRect (236,0,35,12);
         lblHr.Frame = new CoreGraphics.CGRect (271,0,35,12);


      }

   }




}

