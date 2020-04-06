using Foundation;
using System;
using UIKit;

namespace TrySpeechPlus
{
   public partial class AboutController : UIViewController {

      public AboutController(IntPtr handle) : base(handle) {



      }



      partial void cmdHelp_TouchUpInside(UIButton sender) {

         UIApplication.SharedApplication.OpenUrl(new NSUrl("http://www.zeemerixdata.com/baseball_ios/help/default.html"));

      }

      partial void cmdVisitSit_TouchUpInside(UIButton sender) {

         UIApplication.SharedApplication.OpenUrl(new NSUrl("http://www.zeemerix.com/"));

      }



   }

}