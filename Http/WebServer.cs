using System;
using System.Collections.Generic;
using System.IO;

using cloudmusic2upnp.Utils;

namespace cloudmusic2upnp.Http
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

		private static string ListenerCallback (System.Net.HttpListenerRequest context)
		{
			string path = context.Url.AbsolutePath;
			string content = "";

			if (path == "/" || path == "/index.html" || path == "/index.htm") {
				content = System.IO.File.ReadAllText (Path.Combine ("Http", "index.html"), new System.Text.UTF8Encoding ());
			}
			return content;
		}

		public void Stop ()
		{
			runner.Stop ();
		}
	}
}
