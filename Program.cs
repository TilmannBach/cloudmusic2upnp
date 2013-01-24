using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
// using OpenSource.UPnP;
// using Mono.Upnp;

namespace cloudmusic2upnp
{
    class Program
    {
        /// <summary>
        /// Implementation test for Mono.Upnp
        /// </summary>
        /// <param name="args">none</param>
        static int Main(string[] args)
        {
            try
            {
                new Core();
            }
            catch (Exception ex)
            {
                if (ex.InnerException is HttpListenerException)
                    if (((HttpListenerException)ex.InnerException).ErrorCode == 5)
                    {
                        Console.Error.WriteLine("Access Denied. Administrator permissions are " +
                            "required to use the selected options. Use an administrator shell " +
                            "to complete these tasks.");
                        return 740; // ERROR_ELEVATION_REQUIRED
                    }
            }
            return 0;
        }
    }
}