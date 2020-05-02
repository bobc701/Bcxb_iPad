using Foundation;
using System;
using UIKit;

using Xamarin.Essentials;


namespace TrySpeechPlus
{
   public partial class AboutController : UIViewController {

      public AboutController(IntPtr handle) : base(handle) {



      }


      public override void ViewDidLoad() {
         // ----------------------------------
         base.ViewDidLoad();

         // #2004.01 - Version tracking w/ Xamarin Essentials...
         // So now you can just update ver & bld in the plist forget about the StoryBoard.
         var ver = VersionTracking.CurrentVersion;
         var bld = VersionTracking.CurrentBuild;
         lblVersion.Text = $"Version {ver}, Build {bld}";

      }





      partial void cmdHelp_TouchUpInside(UIButton sender) {

         UIApplication.SharedApplication.OpenUrl(new NSUrl("http://www.zeemerixdata.com/baseball_ios/help/default.html"));

      }

      partial void cmdVisitSit_TouchUpInside(UIButton sender) {

         UIApplication.SharedApplication.OpenUrl(new NSUrl("http://www.zeemerix.com/"));

      }



   }

}