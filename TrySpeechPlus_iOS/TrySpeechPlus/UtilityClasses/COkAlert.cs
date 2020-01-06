using System;
using Foundation;
using UIKit;


namespace BCX.BCXCommon
{
   public static class CAlert {


      public static void ShowOkAlert(
         string title, string msg, string lblOk, 
         Action<UIAlertAction> okAction, UIViewController ctlr) {
      // -----------------------------------------------------------------------
      // In this version of ShwOkAlert, you supply an action method that will be 
      // invoked when user dismisses the action.
      // -----------------------------------------------------------------------
         UIAlertController alertController; 
         //string titleUse = title == "" ? "Error" : title; 
         alertController = UIAlertController.Create(title, msg, UIAlertControllerStyle.Alert);
         alertController.AddAction(UIAlertAction.Create(lblOk, UIAlertActionStyle.Default, okAction));
         ctlr.PresentViewController(alertController, true, null);
      }


      public static void ShowOkAlert(
         string title, string msg, string lblOk, 
         UIViewController ctlr) {
      // -----------------------------------------------------------------------
      // In this version of ShwOkAlert, you supply no action, so no action occurs
      // when user dismissed the alert.
      // -----------------------------------------------------------------------
         UIAlertController alertController; 
         alertController = UIAlertController.Create(title, msg, UIAlertControllerStyle.Alert);
         alertController.AddAction(UIAlertAction.Create(lblOk, UIAlertActionStyle.Default, alert => { }));
         ctlr.PresentViewController(alertController, true, null);
      }


      public static void ShowYesNoAlert(
         string title, string msg, string lblYes, string lblNo, 
         Action<UIAlertAction> yesAction, Action<UIAlertAction> noAction, UIViewController ctlr) {
      // -------------------------------------------------------------------
      // This shows 2 buttons which you can customize.
      // For no action (on either of the buttons) pass 'alert => {}'.
      // -------------------------------------------------------------------

         UIAlertController alert = UIAlertController.Create(title, msg, UIAlertControllerStyle.Alert);
         //alert.PopoverPresentationController.SourceView = ctlr.View;  
         //alert.PopoverPresentationController.SourceRect = ctlr.View.Bounds;

         alert.AddAction(UIAlertAction.Create(lblYes, UIAlertActionStyle.Default, yesAction));
         alert.AddAction(UIAlertAction.Create(lblNo, UIAlertActionStyle.Cancel, noAction));
         ctlr.PresentViewController(alert, true, null);

      }


      public static void ShowActionSheet(
         string title, string msg, string[] labels,  
         Action<UIAlertAction>[] actions, UIViewController ctlr) {
      // -------------------------------------------------------------------

         UIAlertController alert = UIAlertController.Create(title, msg, UIAlertControllerStyle.ActionSheet);
         if (alert.PopoverPresentationController != null) {
            alert.PopoverPresentationController.SourceView = ctlr.View;
            //alert.PopoverPresentationController.SourceRect = ctlr.View.Bounds;
         } 
         for (int i = 0; i < labels.Length; i++) 
            alert.AddAction(UIAlertAction.Create(labels[i], UIAlertActionStyle.Default, actions[i]));
         ctlr.PresentViewController(alert, true, null);

      }


   }

}

