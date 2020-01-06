using System;
using Foundation;
using UIKit;

namespace BCX.BCXB {

   public class CLineupCardSource : UITableViewSource {

   // Members
   // ---------------
      private CGame gm;   
      private int abThis;
      private CLineupCard lineupCard;
      private NSString cellIdentifier = (NSString)"LineupCard";
      private LineupCardController fLineup;

//      public int SelectedRow = -1;
//      public CBatter SelectedBatter = null;



   // Constructor...
      public CLineupCardSource (int ab1, LineupCardController fLineup1) {
      // ----------------------------------------------------------
         abThis = ab1;
         fLineup = fLineup1;
         gm = fLineup1.g;
         lineupCard = fLineup1.lineupCard;
      }


      public override nint NumberOfSections(UITableView tableView) {
      // -----------------------------------------------------------
         return 1;
      } 


      public override nint RowsInSection (UITableView tableview, nint section) {
      // --------------------------------------------------------------------------
         if (gm.UsingDh) return 10;
         else return 9;
      }


      public override UIView GetViewForHeader(UITableView tableView, nint section) {
      // ---------------------------------------------------------------------------

         CSectionHeader_Lineup hdrL = 
            (CSectionHeader_Lineup)tableView.DequeueReusableHeaderFooterView(new NSString("TableHeaderL"));
         if (hdrL == null) {
            hdrL = new CSectionHeader_Lineup(new NSString("TableHeaderL"));
         }
         return hdrL;

      }


      public override string TitleForHeader(UITableView tableView, nint section) {
      // ----------------------------------------------------------------------
      // We use custom header class, so this is blank...
         return ""; 

      }


      public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) { 
      // ----------------------------------------------------------------------
         var cell = tableView.DequeueReusableCell(cellIdentifier) as CLineupCardCell;  
         if (cell == null)
            cell = new CLineupCardCell((NSString)"LineupCard");   
         cell.UpdateCell(gm, abThis, lineupCard, indexPath);
         return cell;

      }


      public override nfloat GetHeightForHeader(UITableView tableView, nint section) {
      // -----------------------------------------------------------------------------
      // This overrides SectionHeaderHeight (of UITableView), for indiv rows...
         return (nfloat)40.0;
      }


      public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath) {
      // ------------------------------------------------------------------------------
      // This overrides RowHeight (of UITableView), for indiv rows...
         return (nfloat)30.0;
      }

      public override void RowSelected(UITableView tableView, NSIndexPath indexPath) {
   // ------------------------------------------------------------------------------
         int r = indexPath.Row;
         fLineup.RowSelected(r, lineupCard.CurrentLineup[r]);

      }

   }



}

