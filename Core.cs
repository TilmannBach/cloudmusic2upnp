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
        public UserInterface.Web.Interface WebInterface;
        private bool shutdownPending = false;

        /// <summary>
        /// 
        /// </summary>
        public Core()
        {
            Utils.Logger.Log(Utils.Logger.Level.Info, "cloudmusic2upnp version " + 
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " started"
            );

            HackMonoProxyIssue();

            UPnP = new DeviceController.UPnP.Controller();
            Providers = new ContentProvider.Providers();
            WebInterface = new UserInterface.Web.Interface(UPnP, Providers);
            WebInterface.Start();


            // catch Strg+C, console quit's and SIGKILL's and free up the C++-UPnP-lib first
            AppDomain appDomain = AppDomain.CurrentDomain;
            appDomain.ProcessExit += new EventHandler(OnShutdownRequest);
            Console.CancelKeyPress += OnShutdownRequest;
        }

        void OnShutdownRequest(object sender, EventArgs e)
        {
            if (!shutdownPending)
            {
                shutdownPending = true;
                WebInterface.Stop();
                UPnP.Shutdown();
                Utils.Logger.Log(Utils.Logger.Level.Info, "Good bye.");
                Utils.Config.Save();
            }
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
        private void HackMonoProxyIssue()
        {
            int p = (int)Environment.OSVersion.Platform;
            if ((p == 4) || (p == 128))
            { // Running on Unix
                string proxy = Environment.GetEnvironmentVariable("http_proxy");
                if (proxy != "" && proxy != null)
                {
                    WebRequest.DefaultWebProxy = new WebProxy(proxy);
                }
            }

        }
    }
}
