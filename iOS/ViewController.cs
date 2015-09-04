using System;
		
using UIKit;

using SplunkClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DataSender.iOS
{
	public partial class ViewController : UIViewController
	{

		public ViewController (IntPtr handle) : base (handle)
		{		
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Perform any additional setup after loading the view, typically from a nib.
			Button.AccessibilityIdentifier = "Splunk>®";
			SplunkLogger spl = new SplunkLogger ("http://10.80.8.76:8088/services/collector",
				"9658F9CB-796C-4C0B-A1EC-84CEA4B9F768", false);
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

		public override void DidReceiveMemoryWarning ()
		{		
			base.DidReceiveMemoryWarning ();		
			// Release any cached data, images, etc that aren't in use.		
		}
	}
}
