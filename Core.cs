using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace cloudmusic2upnp
{
	public class Core
	{
		public DeviceController.IController UPnP;
		public ContentProvider.Providers Providers;
		public UserInterface.UIProxy UI;
        public Http.WebServer WebServer;

		/// <summary>
		/// 
		/// </summary>
		public Core ()
		{
			Logger.Log (Logger.Level.Info, "cloudmusic2upnp version " + 
				System.Reflection.Assembly.GetExecutingAssembly ().GetName ().Version.ToString () + " started"
			);

            WebServer = new Http.WebServer();
            try
            {
                WebServer.Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }

			HackMonoProxyIssue ();

			UPnP = new DeviceController.UPnP ();
			Providers = new ContentProvider.Providers ();
			UI = new UserInterface.UIProxy (UPnP, Providers);
			UI.Start ();


			//Console.ReadLine ();
            AppDomain appDomain = AppDomain.CurrentDomain;
            appDomain.ProcessExit += new EventHandler(app_ProcessExit);

            Console.CancelKeyPress += Console_CancelKeyPress;
		}

        void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            UPnP.FreeAll();
        }

        private void app_ProcessExit(object sender, EventArgs e)
        {
            UPnP.FreeAll();
        }

		/// <summary>
		/// 	Hacks the mono proxy issue.
		/// </summary>
		/// <description>
		/// 	This issue happens, if you are using:
		/// 		1. Mono <= 2.10
		///         2. Linux
		/// 	    3. a proxy
		/// 	In this case it would throw an exception, because of a Mono
		/// 	bug. With setting this proxy manually it will be avoided.
		/// </description>
		private void HackMonoProxyIssue ()
		{
			int p = (int)Environment.OSVersion.Platform;
			if ((p == 4) || (p == 128)) { // Running on Unix
				string proxy = Environment.GetEnvironmentVariable ("http_proxy");
				if (proxy != "" && proxy != null) {
					WebRequest.DefaultWebProxy = new WebProxy (proxy);
				}
			}

		}
	}
}
