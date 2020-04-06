#define IOS

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Net;


namespace BCX.BCXCommon {

   public static class GFileAccess {
      /* ---------------------------------------------------------------------
       * This serves up file objects for the rest of the app. It is anticipated 
       * that this will need to have a separate version for each platform.
       * ---------------------------------------------------------------------*/

      //Locations
      //---------
#if IOS
      private static string appRoot; //For iOS, fill these from a ViewDidLoad.
      private static string docFolder;
#endif

      public static string ModelFolder; // = @"C:\@\Dropbox\Prj\BCX\Engine\Model\Compile";
      public static string TeamFolder; // = @"C:\@\Dropbox\Prj\BCX\PlayerData\Teams";
      public static string ResultsFolder;

      private static string OrgName = "Zeemerix";
      private static string ProductName = "Zeemerix Baseball";


#if IOS
      public static void SetFolders(string appRoot1, string docFolder1) {
      // ----------------------------------------------------------------
      // These need to be passed in since not available to the library...
         appRoot = appRoot1;
         docFolder = docFolder1;


         // For IOS...

         TeamFolder = Path.Combine(appRoot, "Teams");
         if (!Directory.Exists(TeamFolder)) 
            throw new Exception (TeamFolder + " does not exists. Reinstall " + ProductName);

         ModelFolder = Path.Combine(appRoot, "Model");
         if (!Directory.Exists(ModelFolder)) 
            throw new Exception (ModelFolder + " does not exists. Reinstall " + ProductName);
 
      // Results folder may or may not exist, so create it not...
         ResultsFolder = Path.Combine(docFolder, "Results");
         try {
            if (!File.Exists(ResultsFolder)) Directory.CreateDirectory(ResultsFolder);
         }
         catch (Exception ex) {
            string msg =
               "Could not create folder: " + ResultsFolder + "\r\n" +
               "Error: " + ex.Message;
            throw new Exception (msg);
         }
      }

#endif

    
#if WINDOWS

      public static void SetFolders() {
      // ------------------------------------------------------------------------------------------
         Environment.SpecialFolder dsk;

         dsk = Environment.SpecialFolder.CommonApplicationData; // This evaluates to ProgramData
         TeamFolder = Environment.GetFolderPath(dsk) + "\\" + OrgName + "\\" + ProductName + "\\Teams"; 
         if (!Directory.Exists(TeamFolder)) {
            throw new Exception (TeamFolder + " does not exists. Reinstall " + ProductName);
         }

         dsk = Environment.SpecialFolder.CommonApplicationData; // This evaluates to ProgramData
         ModelFolder = Environment.GetFolderPath(dsk) + "\\" + OrgName + "\\" + ProductName + "\\Model";
         if (!Directory.Exists(TeamFolder)) {
            throw new Exception (ModelFolder + " does not exists. Reinstall " + ProductName);
         }

         try { 
       //dsk = Environment.SpecialFolder.MyDocuments; // Users\<userid>\Documents
         dsk = Environment.SpecialFolder.CommonApplicationData; // This evaluates to ProgramData
         ResultsFolder = Environment.GetFolderPath(dsk) + "\\" + OrgName + "\\" + ProductName + "\\Results";

         // Check for existance of ResultsFolder & create if necessary...
            if (!Directory.Exists(ResultsFolder)) {
               Directory.CreateDirectory(ResultsFolder);
               Debug.Print("Created Results folder: " + ResultsFolder);
            }
         }
         catch (Exception ex) {
            string msg =
               "Could not create folder: " + ResultsFolder + "\r\n" +
               "Error: " + ex.Message;
            throw new Exception (msg);
         }

      }

#endif


      public static StreamReader GetModelFile(short engNum) {
      // ------------------------------------------------------------------
      // This returns a file object for CFEng1,2 oe 3.
      // ------------------------------------------------------------------
         string path1 = Path.Combine(ModelFolder, "cfeng[1].bcx");
         try {
            path1 = path1.Replace("[1]", engNum.ToString());
            StreamReader f = new StreamReader(path1);
            return f;
         }
         catch (Exception ex) {
            string msg = "Could not open " + path1 + "\r\nError: " + ex.Message;
            throw new Exception(msg);
         }
         
      }
   
 
      /// <summary>
      /// This serves up a file object for a team file, for reading, with bcxt extention
      /// </summary>
      /// ------------------------------------------------------------------------------
      public static StreamReader GetTeamFileReader(string fileName) {
     
         string path1 = Path.Combine(TeamFolder, fileName + ".bcxt");
         try {
            if (!File.Exists(path1)) return null;
            StreamReader f = new StreamReader(path1);
            return f;
         }
         catch (Exception ex) {
            string msg = "Could not open " + path1 + " for reading/r/nError: " + ex.Message;
            throw new Exception(msg);
         }
      
      }



      /// <summary>
      /// This serves up a file object for a string file, for reading, with txt extention
      /// fileName arg should include any folder, if any, plus the extention.
      /// Example: "Strings/Fielding1.txt"
      /// </summary>
      /// ------------------------------------------------------------------------------
      public static StreamReader GetOtherFileReader(string fileName) {
     
         string path1 = Path.Combine(appRoot, fileName);
         try {
            if (!File.Exists(path1)) return null;
            StreamReader f = new StreamReader(path1);
            return f;
         }
         catch (Exception ex) {
            string msg = "Could not open " + path1 + " for reading/r/nError: " + ex.Message;
            throw new Exception(msg);
         }
      
      }
      

      /// <summary>
      /// Checks if a team file is installed on user's computer.
      /// Used in building dropdown lists for PickTeams form.
      /// </summary>
      /// 
      public static bool TeamFileInstalled(string fileName) {
      // ----------------------------------------------------
         string path1 = Path.Combine(TeamFolder, fileName + ".bcxt");
         return (File.Exists(path1));

      }


      /// <summary>
      /// This serves up a file object for a team file, for writing, with bcxt extention
      /// </summary>
      ///
      public static StreamWriter GetTeamFileWriter(string fileName) {
     
         string path1 = Path.Combine(TeamFolder, fileName + ".bcxt");
         try {
            var f = new StreamWriter(path1, append:false);
            return f;
         }
         catch (Exception ex) {
            string msg = "Could not open " + path1 + " for writing/r/nError: " + ex.Message;
            throw new Exception(msg);
         }

      }


      // --1909.01 Changes for team data on web.
      // These 3 methods added, replacing earlier...

      public static List<CTeamRecord> GetTeamsInLeague(string league1, out bool dh) {
      // ---------------------------------------------------------------------------
         string rec;
         var teamList = new List<CTeamRecord>();
         try
         {
            using (StringReader f = GetTextFileOnLine(league1 + ".bcxl"))
            {
               //StreamReader f = GetTextFileOnLine(path1); 

               rec = f.ReadLine(); //Skip version #   

               // Does this league & year use the DH rule?
               rec = f.ReadLine(); //Read the league data line
               dh = rec.Substring(rec.Length - 1, 1) == "1"; //Last char is DH

               // Read remaining lines getting list of teams...
               while ((rec = f.ReadLine()) != null)
               {
                  if (rec == "END") break;
                  var a = rec.Split(',');
                  // We only want to add the team to the list if the user has the file
                  // on his system. User might well have only some of the teams 
                  // installed for a league. 
                  // Update: for on-line version, we don't have to worry, can assume the files
                  // are there...
                  //if (GFileAccess.TeamFileIsInstalled(a[0]))
                  teamList.Add(new CTeamRecord()
                  {
                     TeamTag = a[0],
                     LineName = a[1],
                     City = a[2],
                     NickName = a[3],
                     UsesDh = dh
                  });
               }
               f.Close();
            }
            return teamList;
         }
         catch (Exception ex)
         {
            string msg = "Error getting teams in league: " + league1 + "\r\n" + ex.Message;
            throw new Exception(msg);


         }
      }


      public static List<string> GetLeagueList() {
      // --------------------------------------------------------
         try {
            string rec;
            List<string> list = new List<string>();
            StringReader f = GetTextFileOnLine("Leagues");
            while ((rec = f.ReadLine()) != null) {
               list.Add(rec);
            }
            return list;
         }
         catch (Exception ex) {
            string msg = "Error getting list of available leagues: \r\n" + ex.Message;
            throw new Exception(msg);
         }

      }


      public static StringReader GetTextFileOnLine(string fName) {
         // ---------------------------------------------------------------
         //WebClient client = new WebClient(); 
         string path = "";
         path = @"https://www.zeemerixdata.com/BcxbTeams/" + fName + ".txt";

         // ----------------------------------------------------
         // Found this approach on Web.
         // Uses HttpWebRequest i/o WebClient, and so allows you 
         // to set the timeout period.
         // HttpWebRequest --> HttpWebResponse --> Stream --> StreamReader
         // ----------------------------------------------------

         HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
         request.Timeout = 15000;
         request.ReadWriteTimeout = 15000;
         var resp = (HttpWebResponse)request.GetResponse();

         var f = new StreamReader(resp.GetResponseStream());
         string s = f.ReadToEnd();
         return new StringReader(s);
      }




   }


   public struct CTeamRecord {
   // ---------------------------------------------------
      public string TeamTag;
      public string LineName;
      public string City;
      public string NickName;
      public bool UsesDh;
   }

}
