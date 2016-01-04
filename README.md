# Splunk Logger PCL
This lightweight logger was developed to provide an easy way to log events to a Splunk instance through the Http Event Collector data input.  Since it’s a portable class library, it can be used with any .NET application.  More importantly, it can be used within the Xamarin platform (Xamarin Studio or Xamarin tools within Visual Studio) to allow easy event logging from cross-platform mobile applications!

# How-to:
## INSTALLATION

- Clone this git repository, and add a reference in your project to the SplunkClient  project.
- Make sure to add a using statement to the top of the classes you’d like to log events from: `using SplunkClient;`

## LOGGING EVENTS
After you’ve referenced the SplunkClient project and added your using statements, the next step is to construct a SplunkLogger.  **The first thing the constructor method takes** is a URI string of the Splunk instance you’re sending data to, for example: `http://localhost:8088/services/collector`

8088 is the default port for the HTTP Event Collector, and `/services/collector` is the rest of the path to the collector.  These may be different depending on your Splunk deployment or system admin’s preferences.

**The next thing the constructor takes** is a unique token GUID string, that can be generated when you add inputs to the Http Event Collector.  This can be done from Splunk’s UI (settings > data inputs > Http Event Collector > New Token) or Splunk’s command line interface. **The next thing the constructor takes** is a boolean indicating whether or not the request requires SSL.

Once the SplunkLogger is constructed, you can simply call `myLogger.Log();` for normal synchronous logging, or `myLogger.LogAsync();` for asynchronous logging.  This means that calling `myLogger.LogAsync();` will not interrupt the thread it is called from.  This is extremely helpful in mobile applications, where you don’t want the user interface thread to experience any interruptions.

Also, call `myLogger.EnableBatching();` if you want SplunkLogger to package up the individual events your application sends, and send them as batches in less frequent requests.  This can help your application’s performance if you find yourself trying to send lots of events at once.  To disable, call `myLogger.DisableBatching();`

### EXAMPLE CODE

```csharp
using SplunkClient;

namespace MyApplication
{
	public class MyClass
	{
	//Can also construct SplunkLogger here if you want to log from different methods

		public static void SomeMethod()
		{
			SplunkLogger spl = new SplunkLogger ("http://localhost:8088/services/collector",
				"81EBB9BA-BEAC-43AF-B482-F683CFBBE68C", false);
			spl.Log(“My First Event”);
		}

		public static async void SomeUIHandlerMethod()
		{
			SplunkLogger spl = new SplunkLogger ("http://localhost:8088/services/collector",
				"81EBB9BA-BEAC-43AF-B482-F683CFBBE68C", false);
			await spl.LogAsync(“My First Non-blocking Event”);
		}
	}
}
```
