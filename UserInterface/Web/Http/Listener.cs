using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace cloudmusic2upnp.UserInterface.Web.Http
{
    class Listener
    {
        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerRequest, string> _responderMethod;

        public Listener(string[] prefixes, Func<HttpListenerRequest, string> method)
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException(
                    "Needs Windows XP SP2, Server 2003 or later.");

            // URI prefixes are required, for example 
            // "http://localhost:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // A responder method is required
            if (method == null)
                throw new ArgumentException("method");

            foreach (string s in prefixes)
                _listener.Prefixes.Add(s);

            _responderMethod = method;
            try
            {
                _listener.Start();
            }
            catch (HttpListenerException ex)
            {
                // exception when using HttpListener without elevated rights (UAC) on windows operating systems
                if (ex.ErrorCode == 5 && !HasAdministratorPrivileges())
                {
                    Logger.Log(Logger.Level.Error, "Access Denied. Administrator permissions are " +
                        "required to use the HTTP webinterface. Use an administrator " +
                        "command promt to start with the webinterface."
                    );
                }
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                // exception when using port < 1024 on unix operating systems
                if (ex.ErrorCode == 10013)
                {
                    Logger.Log(Logger.Level.Error, "Couldn't start HTTP server. Maybe you need root rights.");
                }
            }
        }

        public Listener(Func<HttpListenerRequest, string> method, params string[] prefixes)
            : this(prefixes, method)
        {
        }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                Console.WriteLine("Webserver running...");
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                string path = _responderMethod(ctx.Request);
                                byte[] content;

                                try
                                {
                                    content = System.IO.File.ReadAllBytes(path);
                                    ctx.Response.ContentType = MimeAssistant.GetMIMEType(Path.GetExtension(path));
                                }
                                catch (FileNotFoundException)
                                {
                                    ctx.Response.StatusCode = 404;
                                    content = new byte[0] { };
                                    Logger.Log(Logger.Level.Warning, "HTTP 404 - File Not Found \"" + ctx.Request.Url.AbsolutePath + "\"");
                                }
                                ctx.Response.ContentLength64 = content.Length;
                                ctx.Response.OutputStream.Write(content, 0, content.Length);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log(Logger.Level.Warning, ex.Message);
                            }
                            finally
                            {
                                // always close the stream
                                ctx.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext());
                    }
                }
                catch
                {
                } // suppress any exceptions
            }
            );
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }


        private static bool HasAdministratorPrivileges()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}