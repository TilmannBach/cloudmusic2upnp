using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

using cloudmusic2upnp.UserInterface.Web;
using cloudmusic2upnp.UserInterface.Web.Protocol;
using cloudmusic2upnp.ContentProvider;

namespace cloudmusic2upnp
{
    public class Core
    {
        public static DeviceController.IController UPnP;
        public static ContentProvider.Providers Providers;
        public static UserInterface.Web.Interface WebInterface;
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

            UPnP = new DeviceController.UPnP.Controller(Utils.Config.Load().LanInterfaceId);
            Providers = new ContentProvider.Providers();
            WebInterface = new UserInterface.Web.Interface(UPnP, Providers);
            WebInterface.Start();


            // catch Strg+C, console quit's and SIGKILL's and free up the C++-UPnP-lib first
            AppDomain appDomain = AppDomain.CurrentDomain;
            appDomain.ProcessExit += new EventHandler(HandleShutdownRequest);
            Console.CancelKeyPress += HandleShutdownRequest;

            WebInterface.OnPlayRequest += HandleOnPlayRequest;
            WebInterface.OnSearchRequest += HandleOnSearchRequest;

            //add all device controller to the playlist singleton
            Playlist.Active.AddDeviceController(UPnP);

            Playlist.Active.ItemAdded += HandlePlaylistChanged;
            Playlist.Active.ItemRemoved += HandlePlaylistChanged;
        }


        void HandlePlaylistChanged(PlayListItem track)
        {
            WebInterface.SendMessageAll(new PlaylistNotification(Playlist.Active));
            Utils.Logger.Log("Sent playlist notification.");
        }


        void HandleOnSearchRequest(IWebClient client, SearchRequest request)
        {
            Utils.Logger.Log("Requested search for: '" + request.Query + "'.");

            try
            {
                var tracks = Providers.Plugins["Soundcloud"].Search(request.Query);

                var response = new SearchResponse(request.Query, tracks);
                client.SendMessage(response);

                Utils.Logger.Log("Sent response for search for: '" + response.Query + "'.");
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.Timeout)
                    Utils.Logger.Log("(504) Gateway Timeout to content provider!");
            }
        }


        void HandleOnPlayRequest(IWebClient client, PlayRequest request)
        {
            Utils.Logger.Log("Requested play for: '" + request.Track + "'.");
            Playlist.Active.PlayOrQueue(request.Track);
        }


        void HandleShutdownRequest(object sender, EventArgs e)
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
