using Foundation;
using UIKit;
using System;
using System.Threading;
using BCX.BCXCommon;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Diagnostics;

namespace TrySpeechPlus
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations

		public override UIWindow Window {
			get;
			set;
		}

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			// Override point for customization after application launch.
			// If not required for your application you can safely delete this method

			// Code to start the Xamarin Test Cloud Agent
			// I removed this from proj's conditional compilation constants, because
			// seemed to be causing crash in simulayor. (10/21-16)
#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
#endif

	      PrimeTeamCache(); // We can't await this because overridden method is not async.
			Thread.Sleep(4500); // Delay to show splash longer
			Debug.WriteLine($"TeamCache.Count after FinishedLaunching: {GFileAccess.TeamCache.Count}");
			return true;

		}


		private async Task PrimeTeamCache() {
		// ---------------------------------------------------------
		// This routine will do an initial fill of the teamCache using 2000-2020,
		// while the splash screen is being displayed.
		// If no internet, this will fail and do nothing.

			try {
				var url = new System.Uri(GFileAccess.client.BaseAddress, $"liveteamrdr/api/team-list/2010/2020");

				List<BCX.BCXCommon.CTeamRecord> yearList10;
				HttpResponseMessage response = await GFileAccess.client.GetAsync(url.ToString());
				if (response.IsSuccessStatusCode) {
					yearList10 = await response.Content.ReadAsAsync<List<BCX.BCXCommon.CTeamRecord>>();
				}
				else {
					throw new Exception($"Error loading initial list of teams\r\nStatus code: {response.StatusCode}");
				}
				GFileAccess.TeamCache.AddRange(yearList10);
			}
			catch (Exception ex) {
			// Just do nothing here. Can't show error dialog.
		   // CAlert.ShowOkAlert("Error initializing data", ex.Message, "OK", this);
			}


		}


		public override void OnResignActivation (UIApplication application)
		{
			// Invoked when the application is about to move from active to inactive state.
			// This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
			// or when the user quits the application and it begins the transition to the background state.
			// Games should use this method to pause the game.
		}

		public override void DidEnterBackground (UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background exection this method is called instead of WillTerminate when the user quits.
		}

		public override void WillEnterForeground (UIApplication application)
		{
			// Called as part of the transiton from background to active state.
			// Here you can undo many of the changes made on entering the background.
		}

		public override void OnActivated (UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive. 
			// If the application was previously in the background, optionally refresh the user interface.
		}

		public override void WillTerminate (UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
		}
	}
}


