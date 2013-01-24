using System;
using System.Collections.Generic;
using System.IO;

namespace cloudmusic2upnp.Http
{
    public class WebServer
    {
        Listener runner;

        public void Start()
        {
            runner = new Listener(ListenerCallback, "http://*:80/");
            try
            {
                runner.Run();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string ListenerCallback(System.Net.HttpListenerRequest context)
        {
            string path = context.Url.AbsolutePath;
            string content = "";

            if (path == "/" || path == "/index.html" || path == "/index.htm")
            {
                content = System.IO.File.ReadAllText(Path.Combine("http","index.html"), new System.Text.UTF8Encoding());
            }
            return content;
        }

        public void Stop()
        {
            runner.Stop();
        }
    }
}
