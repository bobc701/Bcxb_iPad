using System;
using UIKit;
using Foundation;

namespace BCX.BCXB  {


   public struct CStats_B {
      // =========================
         public string bname;
         public int ab;
         public int r;
         public int h;
         public int bi;
         public int b2;
         public int b3;
         public int hr;
         public int bb;
         public int so;
      }

      public struct CStats_P {
      // =========================
         public string pname;
         public int ip3; //This is the number of 1/3 innings
         public int h;
         public int r;
         public int hr;
         public int er;
         public int bb;
         public int so;
      }


   public class CBoxScoreSource : UITableViewSource {

   // Members
   // ---------------
      private CGame gm;
      private int abThis;
      private NSString cellIdentifier = (NSString)"TableCellP";


   // Constructor...
      public CBoxScoreSource (CGame g1, int ab1) {
      // ----------------------------------------------------------
         gm = g1;
         abThis = ab1;

      }

      public override nint NumberOfSections(UITableView tableView) {
      // -----------------------------------------------------------
         return 2;
      } 


      public override nint RowsInSection (UITableView tableview, nint section) {
      // --------------------------------------------------------------------------
         switch (section) {
            case 0: return gm.BBoxLim(abThis) + 1; 
            case 1:return gm.PBoxLim(abThis) + 1; 
            default: return 0;
         }
      }


      public override UIView GetViewForHeader(UITableView tableView, nint section) {
      // ---------------------------------------------------------------------------

         switch (section) {
            case 0: 
               CSectionHeader_B hdrB = 
                  (CSectionHeader_B)tableView.DequeueReusableHeaderFooterView(new NSString("TableHeaderB"));
               if (hdrB == null) {
                  hdrB = new CSectionHeader_B(new NSString("TableHeaderB"));
               }
               return hdrB;

            case 1: 
               CSectionHeader_P hdrP = (CSectionHeader_P)tableView.DequeueReusableHeaderFooterView(new NSString("TableHeaderP"));
               if (hdrP == null) {
                  hdrP = new CSectionHeader_P(new NSString("TableHeaderP"));
               }
               return hdrP;

            default: return null;

         }
      }


      public override string TitleForHeader(UITableView tableView, nint section) {
      // ----------------------------------------------------------------------
      // Note: Titles, Batting, Pitching, are part of custome header views.
         switch (section) {
            case 0: return ""; 
            case 1: return ""; 
            default: return "";
         }
      }


      public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) {
      // ----------------------------------------------------------------------
         if (indexPath.Section == 0) {
            var cell = tableView.DequeueReusableCell(cellIdentifier) as CBoxScoreLine_Batting;
            if (cell == null)
               cell = new CBoxScoreLine_Batting((NSString)"TableCellB");  
            cell.UpdateCell(gm, abThis, indexPath);
            return cell;
         } 
         else {
            var cell = tableView.DequeueReusableCell(cellIdentifier) as CBoxScoreLine_Pitching;
            if (cell == null)
               cell = new CBoxScoreLine_Pitching((NSString)"TableCellP");  
            cell.UpdateCell(gm, abThis, indexPath);
            return cell;
         }

      }


      public override nfloat GetHeightForHeader(UITableView tableView, nint section) {
      // -----------------------------------------------------------------------------
      // This overrides SectionHeaderHeight (of UITableView), for indiv rows...
         return (nfloat)18.0;
      }


      public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath) {
      // ------------------------------------------------------------------------------
      // This overrides RowHeight (of UITableView), for indiv rows...
         return (nfloat)15.0;
      }

   }
}

