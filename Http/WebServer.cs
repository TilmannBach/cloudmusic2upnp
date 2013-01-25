using System;
using System.Collections.Generic;
using System.IO;

using cloudmusic2upnp.Utils;

namespace cloudmusic2upnp.Http
{
	public class WebServer
	{
		Listener runner;

		public void Start ()
		{
			var cfg = Config.Load ();

			try {
				runner = new Listener (ListenerCallback, "http://*:" + cfg.HttpPort + "/");
				runner.Run ();

			} catch (System.Net.Sockets.SocketException ex) {
				if (ex.ErrorCode == 10013) {
					Logger.Log (Logger.Level.Error, "Couldn't start HTTP server. Maybe you need root rights.");
				}

			} catch (Exception ex) {
				throw ex;
			}
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
