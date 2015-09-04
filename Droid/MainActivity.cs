using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using System.Diagnostics;
using System.Threading.Tasks;
using SplunkClient;

namespace DataSender.Droid
{
	[Activity (Label = "DataSender.Droid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			//Start sending random events
			SplunkLogger spl = new SplunkLogger ("http://10.80.8.76:8088/services/collector",
				"81EBB9BA-BEAC-43AF-B482-F683CFBBE68C", false);
			spl.EnableBatching ();
			SendRandomEvents (spl);
		}

		async private Task SendRandomEvents(SplunkLogger spl)
		{
			Stopwatch timer = new Stopwatch ();
			timer.Start ();
			Random r = new Random();
			while (true)
			{
				int waitTimeMillis = r.Next(250, 1500);
				await Task.Delay(waitTimeMillis);

				int numEvents = r.Next (1,10);
				for (int i = 1; i <= numEvents; i++) {
					await spl.LogAsync ("This is event " + i + " out of " + numEvents + ", sent " +
						timer.ElapsedMilliseconds + " milliseconds after application started");
				}
			}
		}
	}
}


