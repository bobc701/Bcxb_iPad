using System;
using UIKit;
using Foundation;

namespace BCX.BCXB
{
   public class CBoxScoreLine_Batting : UITableViewCell {


   // Members
   // -------
      UILabel lblBName, lblAb, lblR, lblH, lblRbi, lblB2, lblB3, lblHr, lblBb, lblSo;
      UIImageView imageView;



   // Constructor:
      public CBoxScoreLine_Batting(Foundation.NSString cellId) : base (UITableViewCellStyle.Default, cellId) {
      // ---------------------------------------------------------------------

         SelectionStyle = UITableViewCellSelectionStyle.Gray;
         ContentView.BackgroundColor = UIColor.White;
         imageView = new UIImageView();


         lblBName = new UILabel () {
            Font = UIFont.FromName("Arial", 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Left,
            BackgroundColor = UIColor.White
         };
         lblAb = new UILabel () {
            Font = UIFont.FromName("AmericanTypewriter", 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White
         };
         lblR = new UILabel () { 
            Font = UIFont.FromName("AmericanTypewriter", 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White
         };
         lblH = new UILabel () {
            Font = UIFont.FromName("AmericanTypewriter", 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White
         };
         lblRbi = new UILabel () { 
            Font = UIFont.FromName("AmericanTypewriter", 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White
         }; 
         lblB2 = new UILabel () { 
            Font = UIFont.FromName("AmericanTypewriter", 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White
         }; 
         lblB3 = new UILabel () { 
            Font = UIFont.FromName("AmericanTypewriter", 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White
         }; 
         lblHr = new UILabel () { 
            Font = UIFont.FromName("AmericanTypewriter", 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White
         }; 
         lblBb = new UILabel () { 
            Font = UIFont.FromName("AmericanTypewriter", 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White
         }; 
         lblSo = new UILabel () { 
            Font = UIFont.FromName("AmericanTypewriter", 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White
         }; 

         ContentView.AddSubviews(new UIView[] {
            lblBName, lblAb, lblR, lblH, lblRbi, lblB2, lblB3, lblHr, lblBb, lblSo});

      }


      public void UpdateCell(CGame gm1, int ab, NSIndexPath ixp) {
      // --------------------------------------------------
         nint bx = gm1.t[ab].xbox[ixp.Row+1];
         nint section = ixp.Section;  
         CBatBoxSet bs1;

         if (ixp.Row <= gm1.BBoxLim(ab)-1) {
         // This is a player row, not the total row...
            bs1 = gm1.t[ab].bat[bx].bs;
            lblBName.Text = bs1.boxName; //stats1[r].bname;
            lblAb.Text = bs1.ab.ToString();
            lblH.Text = bs1.h.ToString();
            lblR.Text = bs1.r.ToString();
            lblRbi.Text = bs1.bi.ToString();
            lblB2.Text = bs1.b2.ToString();
            lblB3.Text = bs1.b3.ToString();
            lblHr.Text = bs1.hr.ToString();
            lblBb.Text = bs1.bb.ToString();
            lblSo.Text = bs1.so.ToString();

         } 
         else {
         // This is the total row, so add things up...
            var tot = new CStats_B {ab=0, r=0, h=0, bi=0, b2=0, b3=0, hr=0, bb=0, so=0};
            for (int i=1; i<=gm1.BBoxLim(ab); i++) {
               bx = gm1.t[ab].xbox[i];      //Added 2/26'19
               bs1 = gm1.t[ab].bat[bx].bs;  //Index changed to bx 2/26'19
               tot.ab += bs1.ab;
               tot.r += bs1.r;
               tot.h += bs1.h;
               tot.bi += bs1.bi;
               tot.b2 += bs1.b2;
               tot.b3 += bs1.b3;
               tot.hr += bs1.hr;
               tot.bb += bs1.bb;
               tot.so += bs1.so;
            }
            lblBName.Text = "Totals";
            lblAb.Text = tot.ab.ToString();
            lblH.Text = tot.h.ToString();
            lblR.Text = tot.r.ToString();
            lblRbi.Text = tot.bi.ToString();
            lblB2.Text = tot.b2.ToString();
            lblB3.Text = tot.b3.ToString();
            lblHr.Text = tot.hr.ToString();
            lblBb.Text = tot.bb.ToString();
            lblSo.Text = tot.so.ToString();



         }

      }

      public override void LayoutSubviews() {
      // ------------------------------------------------
         base.LayoutSubviews ();
         //imageView.Frame = new CoreGraphics.CGRect (0, 0, 40, 12);
         lblBName.Frame = new CoreGraphics.CGRect (0, 0, 70, 12);
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
}

