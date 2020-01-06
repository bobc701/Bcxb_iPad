using System;
using System.Collections.Generic;
using UIKit;
using Foundation;

namespace BCX.BCXCommon {


   public class CSimplePicker {
   // -----------------------------------------------------------------------------
      public event Action<int, string> PickerChanged; 

      public int selectedIx { get; set;} 
      public string selectedText { get; set;}

      private string[] pickList;
      private UITextField txt;


      public CSimplePicker(string[] pickList1, UITextField txt1) {
      // -------------------------------------------------------------------------

         pickList = pickList1;
         txt = txt1;

         selectedIx = 0;
         selectedText = pickList[0];

         var modelBottom = new CSimplePickerModel(pickList);
         modelBottom.PickerChanged += delegate(int p, string txt) {
            selectedIx = p;
            selectedText = txt;
            this.PickerChanged(p, txt);
         };

      // Setup picker view...
         UIPickerView pickerBottom = new UIPickerView();
         pickerBottom.ShowSelectionIndicator = true;
         pickerBottom.Model = modelBottom;

//   // Set up the picker's toolbar...
//      var tb = new UIToolbar();
//      tb.BarStyle = UIBarStyle.Black;
//      tb.Translucent = true;
//      tb.SizeToFit();

//   // Create 'Done' button and add it to the toobar...
//      var cmdDone = new UIBarButtonItem("Done", UIBarButtonItemStyle.Done,
//         (s, e) => {
//            selectedIx = modelBottom.selectedPosIx;
//            this.cboPos.Text = modelBottom.selectedPosAbbr;
//            this.cboPos.ResignFirstResponder();
//         });
//      tb.SetItems(new UIBarButtonItem[]{cmdDone}, true);

      // Tell the textbox to use the picker for input & display toolbar 
      // over it...
         this.txt.InputView = pickerBottom;
   //      this.cboPos.InputAccessoryView = tb;
          
      }

   }


   public class CSimplePickerModel : UIPickerViewModel {
   // ***********************************************

      public string selectedText; // Output: Text of selected item
      public int selectedIx; // Output: Index of selected item

      //private bool usingDh;
      public event Action<int, string> PickerChanged; 
      private string[] pickList; 

  
   // Constructor...
      public CSimplePickerModel (string[] pickList1) {
      // ----------------------------
         pickList = pickList1;
         selectedIx = 0;
         selectedText = pickList[0];

      }


      public override nint GetComponentCount (UIPickerView picker) {
      // ---------------------------------------------------------
         return 1;
      }


      public override nint GetRowsInComponent (UIPickerView picker, nint component) {
      // --------------------------------------------------------------------------
         return pickList.Length;
      }


      public override string GetTitle (UIPickerView picker, nint row, nint component) {
      // ----------------------------------------------------------------------------
         if (row < 0 || row > pickList.Length - 1) return "";
         return pickList[row];
      }


      public override void Selected (UIPickerView picker, nint row, nint component) {
      // ---------------------------------------------------------------------------
         selectedIx = (int)row;
         selectedText = pickList[row];
         if (PickerChanged != null) 
            PickerChanged((int)row, pickList[row]);
      }


   }

}
