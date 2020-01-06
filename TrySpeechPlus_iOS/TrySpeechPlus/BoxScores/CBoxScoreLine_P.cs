using System;
using UIKit;
using Foundation;

namespace BCX.BCXB {

   public class CBoxScoreLine_Pitching : UITableViewCell {


   // Members
   // -------
      UILabel lblPName, lblIp, lblR, lblEr, lblH, lblBb, lblSo, lblHr;
      UIImageView imageView;


   // Constructor:
      public CBoxScoreLine_Pitching(Foundation.NSString cellId) : base (UITableViewCellStyle.Default, cellId) {
      // ---------------------------------------------------------------------

         SelectionStyle = UITableViewCellSelectionStyle.Gray;
         ContentView.BackgroundColor = UIColor.White;
         imageView = new UIImageView();
         string fontName = "AmericanTypewriter";



         lblPName = new UILabel () {
            Font = UIFont.FromName("Arial", 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Left,
            BackgroundColor = UIColor.White
         };
         lblIp = new UILabel () {
            Font = UIFont.FromName(fontName, 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White
         };
         lblR = new UILabel () { 
            Font = UIFont.FromName(fontName, 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White
         };
         lblH = new UILabel () {
            Font = UIFont.FromName(fontName, 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White
         };
         lblEr = new UILabel () { 
            Font = UIFont.FromName(fontName, 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White
         }; 
         lblBb = new UILabel () { 
            Font = UIFont.FromName(fontName, 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White
         }; 
         lblSo = new UILabel () { 
            Font = UIFont.FromName(fontName, 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White
         }; 
         lblHr = new UILabel () { 
            Font = UIFont.FromName(fontName, 12f),
            TextColor = UIColor.Black,
            TextAlignment = UITextAlignment.Right,
            BackgroundColor = UIColor.White
         }; 

         ContentView.AddSubviews(new UIView[] {lblPName, lblIp, lblR, lblH, lblEr, lblBb, lblSo, lblHr});

      }


      public void UpdateCell(CGame gm1, int fl, NSIndexPath ixp) {
      // --------------------------------------------------
         nint px = gm1.t[fl].ybox[ixp.Row+1];
         nint section = ixp.Section;  
         CPitBoxSet ps1;

         if (ixp.Row <= gm1.PBoxLim(fl)-1) {
         // This is a player row, not the total row...
            ps1 = gm1.t[fl].pit[px].ps;
            lblPName.Text = gm1.t[fl].pit[px].pname;  //ps1.boxName;
            lblIp.Text = CGame.StatDisplayStr(ps1.ip3, StatCat.ip);
            lblH.Text = ps1.h.ToString(); 
            lblR.Text = ps1.r.ToString();
            lblEr.Text = ps1.er.ToString();
            lblBb.Text = ps1.bb.ToString();
            lblSo.Text = ps1.so.ToString();
            lblHr.Text = ps1.hr.ToString();
         } 
         else {
         // This is the total row, so add things up...
            var tot = new CStats_P {ip3=0, r=0, h=0, er=0, bb=0, so=0, hr=0};
            for (int i=1; i<=gm1.PBoxLim(fl); i++) {
               px = gm1.t[fl].ybox[i];       //Added 2/26'19
               ps1 = gm1.t[fl].pit[px].ps;   //index chaged to px 2/26'19
               tot.ip3 += ps1.ip3;
               tot.r += ps1.r;
               tot.h += ps1.h;
               tot.er += ps1.er;
               tot.bb += ps1.bb;
               tot.so += ps1.so; 
               tot.hr += ps1.hr; 
            }
            lblPName.Text = "Totals";
            lblIp.Text = CGame.StatDisplayStr(tot.ip3, StatCat.ip);
            lblH.Text = tot.h.ToString();
            lblR.Text = tot.r.ToString();
            lblEr.Text = tot.er.ToString();
            lblBb.Text = tot.bb.ToString();
            lblSo.Text = tot.so.ToString(); 
            lblHr.Text = tot.hr.ToString();
         }

      }


      public override void LayoutSubviews() {
      // ------------------------------------------------
         base.LayoutSubviews ();
         //imageView.Frame = new CoreGraphics.CGRect (0, 0, 40, 12);
         lblPName.Frame = new CoreGraphics.CGRect (0, 0, 70, 12);
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

