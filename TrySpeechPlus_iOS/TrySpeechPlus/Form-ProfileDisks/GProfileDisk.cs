using System;
using UIKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;
using System.Threading.Tasks;

namespace BCX.BCXB {
   
   public class GProfileDisk : UIView {

      //private CGContext ctxt;
      const double pi = Math.PI; 
      const double rInner = 80.0, rOuter = 120.0;


   // Things that need to be set by the caller...
      private double X, Y; //Center of the disc;
      private CParamSet para; //UISearchBarDelegate to extract pcts.
      private double[] pcts;
      private int iFrom, iTo;
      private double pctSpinner = -1.0; //For the spinner. Negative means don't show.
      private CDiceRoll diceRoll;


   // Properties for the label...
      private string profileLabel = ""; //Empty string means don't show label.
      private CGColor profileLabelColor = UIColor.Blue.CGColor;
      private float profileLabelFontSize = 20.0f; //20.0f;

   // Properties for the sub-labels...
      private string subLabel1 = ""; //Used for fielding detail -- good play.
      private string subLabel2 = ""; //Used for fielding detail -- bad play.
      private float subLabelFontSize = 16.0f; //15.0f; //Applies to both sub-labels.

   // Properties for the segments & segment labels...
      private UIColor[] segmentColors;
      private string[] segmentLabels;
      private float segmentLabelFontSize = 12f;

   // Public setter properties...
      public double PctSpinner { set { pctSpinner = value; }}
      public string ProfileLabel { set { profileLabel = value; }}
      public string SubLabel1 { set { subLabel1 = value; }}
      public string SubLabel2 { set { subLabel2 = value; }}
      public CDiceRoll DiceRoll { set { diceRoll = value; }}
      public UIColor[] SegmentColors { set { segmentColors = value; }}
      private string[] SegmentLabels { set { SegmentLabels = value; }}
      public float SubLabelFontSize { set { subLabelFontSize = value; }} //Applies to both sub-labels.
     
      public void SetLabelProperties(CGColor color1, float fontSize1) {
         this.profileLabelColor = color1;
         this.profileLabelFontSize = fontSize1;
      }


      /// <summary>
      /// Initializes a new instance of the <see cref="TrySpeechPlus.CDrawHandler"/> class.
      /// </summary>
      /// --------------------------------------------------------------
      public GProfileDisk (double x1, double y1, CParamSet para1) {

         para = para1;
         X = x1; 
         Y = y1;
         iFrom = 1;
         iTo = para1.SegmentCount;
         pcts = para1.GetWidthArray();
         segmentColors = para1.SegmentColors;
         segmentLabels = para1.SegmentLabels;
      }


      public GProfileDisk() {
      // ----------------------------
         profileLabel = "";

      }


      public void Init (double x1, double y1, CParamSet para1) {
      // -----------------------------------------------------------------------------
      // This is for re-use, so just call constr with no args, and then call this
      // repeatedly as needed, to re-use the same object...
         X = x1; 
         Y = y1;
         iFrom = 1;
         iTo = para1.SegmentCount;
         para = para1;
         pcts = para1.GetWidthArray();
         segmentColors = para1.SegmentColors;
         segmentLabels = para1.SegmentLabels;

      }


      public override void Draw(CGRect rect) {
      // --------------------------------------
      // var alert = UIAlertController.Create ("Draw Pressed", "The Draw button was pressed.", UIAlertControllerStyle.Alert);
      // alert.AddAction (UIAlertAction.Create ("Ok", UIAlertActionStyle.Default, null));
      // PresentViewController (alert, true, null);

         base.Draw (rect);

      // Put this in 'using' block. #1706.27a
         using (var ctxt = UIGraphics.GetCurrentContext ()) {
            ctxt.ScaleCTM (1, -1);
            ctxt.TranslateCTM (0, -Bounds.Height);

            UIColor.Black.SetStroke ();
            UIColor.Red.SetFill ();
            //BCDrawArcSegment(Math.PI/6.0, Math.PI/4.0, UIColor.Purple);  

            ctxt.ClearRect (this.Bounds);
            if (this.pcts == null) return;
            DrawProfileDisk (ctxt);
            if (diceRoll.topLevelResult != TLR.none) {
               pctSpinner = diceRoll.pointOverall;
               DrawSpinner (ctxt);
            }
            WriteDiskLabel (ctxt);
            //await Task.Delay(100);
            //WriteWedgeLabels();

         }

      }


      private void DrawProfileDisk(CGContext ctxt) {
      // ---------------------------------------------------------------
         //double a1 = 3.0 * pi / 2.0;
         double a1 = pi / 2.0;
         double a2;

         for (int i=iFrom; i<=iTo; i++) {
            a2 = a1 - pcts[i] * 2 * pi; 
            BCDrawOneWedge (a1, a2, segmentColors[i], segmentLabels[i], ctxt); 
            a1 = a2; 
         }
            
      }


      private void DrawSpinner(CGContext ctxt) {
      // --------------------------------------------------------------------
         if (pctSpinner < 0.0) return; // If negative, don't draw spinner.

         var path = new CGPath(); 
         path.AddEllipseInRect(new CGRect((nfloat)(X-6.0), (nfloat)(Y-6.0), 12.0, 12.0));

      // Draw the main spinner shaft...
         double a = (pi / 2.0) - (pctSpinner * 2.0 * pi);
         var p0 = new CGPoint(X, Y);
         CGPoint p1 = GetPoint (p0, a, rInner);
         path.MoveToPoint(p1);
         path.AddLineToPoint(p0);

      // Add the tip on the end of the spinner.
         CGPoint p2;
         p2 = GetPoint (p0, a - 0.15, rInner - 15); 
         path.MoveToPoint (p2);
         path.AddLineToPoint(p1);
         p2 = GetPoint (p0, a + 0.15, rInner - 15);
         path.MoveToPoint (p2);
         path.AddLineToPoint(p1);

         path.CloseSubpath(); //Needed?

         UIColor.DarkGray.SetStroke ();
         UIColor.DarkGray.SetFill(); 
         ctxt.SetLineWidth (3);
         ctxt.AddPath (path);
         ctxt.DrawPath (CGPathDrawingMode.EOFillStroke);

       }


      private CGPoint GetPoint(CGPoint p1, double a, double r) {
      // -------------------------------------------------------
      // Return a point offset from p1 at angle a, length r.
         double x1 = p1.X + r * Math.Cos (a);
         double y1 = p1.Y + r * Math.Sin (a);
         return new CGPoint (x1, y1);

      }
         

      private void BCDrawOneWedge(double a1, double a2, UIColor color, string lbl, CGContext ctxt) {
         // ----------------------------------------------------------------
         var path = new CGPath();

         var p0 = new CGPoint(X, Y);
//         var p1 = new CGPoint (x + rOuter * Math.Cos (a1), y + rOuter * Math.Sin (a1)); 
//         var p2 = new CGPoint (x + rOuter * Math.Cos (a2), y + rOuter * Math.Sin (a2));
//         var q1 = new CGPoint (x + rInner * Math.Cos (a1), y + rInner * Math.Sin (a1)); 
//         var q2 = new CGPoint (x + rInner * Math.Cos (a2), y + rInner * Math.Sin (a2));

         CGPoint p1, p2, q1, q2;
         p1 = GetPoint(p0, a1, rOuter);
         p2 = GetPoint(p0, a2, rOuter);
         q1 = GetPoint(p0, a1, rInner);
         q2 = GetPoint(p0, a2, rInner);

         path.MoveToPoint(q1);
         path.AddLineToPoint(p1);
         path.AddArc((nfloat)X, (nfloat)Y, (nfloat)rOuter, (nfloat)a1, (nfloat)a2, true); 
         path.AddLineToPoint(q2);
         path.AddArc((nfloat)X, (nfloat)Y, (nfloat)rInner, (nfloat)a2, (nfloat)a1, false);
                 
         path.CloseSubpath();

         color.SetFill();

         ctxt.AddPath(path);
         ctxt.DrawPath(CGPathDrawingMode.EOFillStroke);

         // Write the segment label, if wedge is wide enough...
         // .35 radians is about 20 deg. This is arbitrary cutoff.
         if (lbl != "" && Math.Abs(a2 - a1) >= 0.35) {
            var pCenter = GetPoint(p0, (a2 + a1) * 0.5F, (rOuter + rInner) * 0.5F);
            WriteWedgeLabel(lbl, pCenter.X - 10.0F, pCenter.Y, ctxt);
         }

      } 

      public void BCDrawRectangle(nfloat x1, nfloat y1, nfloat x2, nfloat y2, CGContext ctxt) {
      // ----------------------------------------------------------------
         var path = new CGPath();
         path.AddLines (new CGPoint[] {
            new CGPoint (x1, y1),
            new CGPoint (x2, y1),
            new CGPoint (x2, y2),
            new CGPoint (x1, y2)
         });

         path.CloseSubpath ();  
         ctxt.AddPath (path);
         ctxt.DrawPath (CGPathDrawingMode.EOFillStroke);
            
      }


      private void WriteDiskLabel(CGContext ctxt) {
         // ----------------------------------------------------------------
         if (profileLabel == "")
            return;

//         gr.ScaleCTM (1, -1);
//         gr.TranslateCTM (0, -Bounds.Height);
//         g.RotateCTM ((nfloat)Math.PI);

         //g.TranslateCTM(0, this.labelFontSize);  
         ctxt.SetLineWidth(1.0f);
         ctxt.SetStrokeColor(this.profileLabelColor); 
         ctxt.SetFillColor(this.profileLabelColor); 
//         UIColor.Cyan.SetStroke ();
//         UIColor.Cyan.SetFill(); 

         ctxt.SetTextDrawingMode(CGTextDrawingMode.FillStroke);
       //ctxt.SelectFont("Helvetica", this.profileLabelFontSize, CGTextEncoding.MacRoman);
         ctxt.SelectFont("Arial", this.profileLabelFontSize, CGTextEncoding.MacRoman);
         ctxt.ShowTextAtPoint((nfloat)(X - rOuter), (nfloat)((Y + rOuter + 10)), profileLabel);

      // Write the sub-labels...
         ctxt.SetStrokeColor(UIColor.Black.CGColor); //Color is fixed.
         //ctxt.SelectFont("Helvetica", this.subLabelFontSize, CGTextEncoding.MacRoman);
         ctxt.SelectFont("Arial", this.subLabelFontSize, CGTextEncoding.MacRoman);
         ctxt.ShowTextAtPoint((nfloat)(X - rOuter), (nfloat)((Y - rOuter - 20)), subLabel1);
         ctxt.ShowTextAtPoint((nfloat)(X - rOuter), (nfloat)((Y - rOuter - 40)), subLabel2);


      } 





//      Take this out -- found better way...
//      private void WriteWedgeLabels() {
//      // ----------------------------------------------------------------
//
//         //double a1 = 3.0 * pi / 2.0;
//         double a1 = pi / 2.0;
//         double a2;
//         var p0 = new CGPoint(X, Y);
//
//         for (int i = iFrom; i <= iTo; i++) {
//            a2 = a1 - pcts[i] * 2 * pi; 
//
//         // Write the segment label, if wedge is wide enough...
//         // .35 radians is about 20 deg. This is arbitrary cutoff.
//            if (segmentLabels[i] != "" && Math.Abs(a2 - a1) >= 0.35) {
//               var pCenter = GetPoint(p0, (a2 + a1) * 0.5F, (rOuter + rInner) * 0.5F);
//               WriteWedgeLabel(segmentLabels[i], pCenter.X - 10.0F, pCenter.Y);
//            }
//            a1 = a2; 
//         }
//
//      }


      private void WriteWedgeLabel(string lbl, nfloat x, nfloat y, CGContext ctxt) {
      // ----------------------------------------------------------------
         if (lbl == "") return;
        
         ctxt.SetLineWidth(1.0f);
         ctxt.SetStrokeColor(UIColor.DarkGray.CGColor); 
         ctxt.SetFillColor(UIColor.DarkGray.CGColor); 
//         UIColor.DarkGray.SetStroke ();
//         UIColor.DarkGray.SetFill(); 

         ctxt.SetTextDrawingMode(CGTextDrawingMode.FillStroke);
         ctxt.SelectFont("Helvetica", this.segmentLabelFontSize, CGTextEncoding.MacRoman);

         ctxt.ShowTextAtPoint(x, y, lbl);

      } 

   }

}

