using System;
using System.Collections.Generic;
using System.IO;
using cloudmusic2upnp.Utils;

namespace cloudmusic2upnp.UserInterface.Web.Http
{
    public class WebServer
    {
        Listener runner;

        public void Start()
        {
            var cfg = Config.Load();

            runner = new Listener(ListenerCallback, "http://*:" + cfg.HttpPort + "/");
            runner.Run();
        }

        private static string ListenerCallback(System.Net.HttpListenerRequest request)
        {
            string[] urlPath = request.Url.AbsolutePath.Split('/');
            if (urlPath[urlPath.Length - 1] == String.Empty)
                urlPath[urlPath.Length - 1] = "index.html";
            string documentRoot = Path.Combine("UserInterface", "Web", "Ressources");
            string documentPath = Path.Combine(documentRoot, Path.Combine(urlPath));

            return documentPath;
        }

        public void Stop()
        {
            runner.Stop();
        }
    }
}