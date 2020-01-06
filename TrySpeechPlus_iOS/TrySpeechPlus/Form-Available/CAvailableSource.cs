using System;
using Foundation;
using UIKit;

namespace BCX.BCXB
{

   public class CAvailableSource : UITableViewSource {

   // Members
   // ---------------
      private CGame gm;   
      private int abThis;
      private CLineupCard lineupCard;
      private NSString cellIdentifier = (NSString)"LineupCard";
      private AvailableController fAvail;



   // Constructor...
      public CAvailableSource (int ab1, AvailableController fAvail1) { //, CLineupCard lineupCard1) {
      // ----------------------------------------------------------
         abThis = ab1;
         fAvail = fAvail1;
         gm = fAvail.g;
         lineupCard = fAvail1.lineupCard;
      }


      public override nint NumberOfSections(UITableView tableView) {
      // -----------------------------------------------------------
         return 1;
      } 


      public override nint RowsInSection (UITableView tableview, nint section) {
      // --------------------------------------------------------------------------
      // Most that can be available is 25 - 9 active = 16.
         return 16;
      }


      public override UIView GetViewForHeader(UITableView tableView, nint section) {
      // ---------------------------------------------------------------------------

         CSectionHeader_Avail hdrA = 
            (CSectionHeader_Avail)tableView.DequeueReusableHeaderFooterView(new NSString("TableHeaderA"));
         if (hdrA == null) {
            hdrA = new CSectionHeader_Avail(new NSString("TableHeaderA"));
         }
         return hdrA;

      }


      public override string TitleForHeader(UITableView tableView, nint section) {
      // ----------------------------------------------------------------------
      // We use a custom header class, so return blank here...
         return ""; 
      }


      public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) { 
      // ----------------------------------------------------------------------
         var cell = tableView.DequeueReusableCell(cellIdentifier) as CAvailableCell;   
         if (cell == null)
            cell = new CAvailableCell((NSString)"TableHeaderA");   
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
         fAvail.SelectedRow = r;
         fAvail.SelectedBatter = lineupCard.Available[r];
      }


   }
}

