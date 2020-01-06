using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using BCX.BCXCommon;

namespace BCX.BCXB
{

   partial class OptionsController : UIViewController
   {

      public CGame gm;
      public CGame.RunMode RunMode { get; set; }
      public bool SpeechOn;



      // Constructor...
      public OptionsController (IntPtr handle) : base (handle)
      {
         // -------------------------------------------------------

      }


      public override void ViewDidLoad ()
      {
         // ----------------------------------
         base.ViewDidLoad ();
         // Perform any additional setup after loading the view, typically from a nib.

         SetSwitches (RunMode);
         SetSpeech (SpeechOn);

         cmdClose.TouchUpInside += delegate (object sender, EventArgs e) {
            // -------------------------------------------------------------
         };

         // In the following ValueChanged handlers for the option switches,
         // if the switch setting is 'Off', we can assume that the othe
         // switches are also off, and so the resultant RunMode s/b 'Normal'...

         optAuto.ValueChanged += delegate (object sender, EventArgs e) {
            // -------------------------------------------------------------
            //SetSwitches();
            if (optAuto.On) { RunMode = CGame.RunMode.Auto; optFast.On = optFastEog.On = false; } else RunMode = CGame.RunMode.Normal;
         };

         optFast.ValueChanged += delegate (object sender, EventArgs e) {
            // -------------------------------------------------------------
            if (optFast.On) { RunMode = CGame.RunMode.Fast; optAuto.On = optFastEog.On = false; } else RunMode = CGame.RunMode.Normal;

         };

         optFastEog.ValueChanged += delegate (object sender, EventArgs e) {
            // -------------------------------------------------------------
            if (optFastEog.On) { RunMode = CGame.RunMode.FastEog; optAuto.On = optFast.On = false; } else RunMode = CGame.RunMode.Normal;

         };

         optSpeech.ValueChanged += delegate (object sender, EventArgs e) {
            // -------------------------------------------------------------
            SpeechOn = optSpeech.On;

         };

         optFastEOP.ValueChanged += delegate (object sender, EventArgs e) {
         // ------------------------------------------------------------------
            if (optFastEOP.On) {
               this.RunMode = CGame.RunMode.FastEOP;
               SpeechOn = optSpeech.On = false;
            }
            else {
               this.RunMode = CGame.RunMode.Normal;
            }

         };


         cmdSaveBoxScore.TouchUpInside += delegate (object sender, EventArgs e) {
            // --------------------------------------------------

            try {
               //var ok = new CAlert();
               CAlert.ShowOkAlert ("Options", "Not implimented in this version", "OK", this);
               //            string fName =
               //               GFileAccess.ResultsFolder + @"\" +
               //               DateTime.Now.ToString("yyyy-MM-dd HH-mm") + " " 
               //               + mGame.city[0] + " at " + mGame.city[1] + ".txt";
               //
               //
               //            mGame.PrintBox(fName);
               //
               //            MessageBox.Show(
               //               "The box score was written to the following file:\r\n" +
               //               fName + "\r\n\r\n" +
               //               //"You can view it by opening the file in NotePad or\n\r" +
               //               //"a word processing program such as Microsoft Word.");
               //               "It will now be opened in Windows Notepad...");
               //
               //         // Open the box score txt file in Notepad...
               //            this.Hide();
               //            System.Diagnostics.Process proc = new System.Diagnostics.Process(); 
               //            proc.EnableRaisingEvents = false; 
               //            proc.StartInfo.FileName = "Notepad.exe"; 
               //            proc.StartInfo.Arguments = fName;
               //            proc.Start(); 


            } catch (Exception ex) {
               //            MessageBox.Show(
               //               "There was an error trying to save the box score.\r\n\r\n" +
               //               "The error was:\r\n" +
               //               ex.Message);
            }

         };

      }


      private void SetSwitches ()
      {
         // --------------------------
         if (optAuto.On) { optFast.On = optFastEog.On = false; RunMode = CGame.RunMode.Auto; } else if (optFast.On) { optAuto.On = optFastEog.On = false; RunMode = CGame.RunMode.Fast; } else if (optFastEog.On) { optAuto.On = optFastEog.On = false; RunMode = CGame.RunMode.FastEog; } else RunMode = CGame.RunMode.Normal;

      }

      private void SetSwitches (CGame.RunMode runMode1)
      {
         // -----------------------------------------------
         RunMode = runMode1;
         optAuto.On = optFast.On = optFastEog.On = false;
         switch (RunMode) {
         case CGame.RunMode.Auto: optAuto.On = true; break;
         case CGame.RunMode.Fast: optFast.On = true; break;
         case CGame.RunMode.FastEog: optFastEog.On = true; break;
         case CGame.RunMode.FastEOP: optFastEOP.On = true; break;
         }
      }


      private void SetSpeech (bool speech1)
      {
         // ---------------------------------------
         SpeechOn = speech1;
         optSpeech.On = speech1;

      }



   }

} 
