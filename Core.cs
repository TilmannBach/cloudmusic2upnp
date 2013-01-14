using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cloudmusic2upnp
{
    public class Core
    {
        /// <summary>
        /// 
        /// </summary>
        public Core()
        {
            Logger.Log("cloudmusic2upnp version " + 
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " started");

            IDeviceController UPnP = new UPnPDeviceController();

            Console.ReadLine();
        }
    }
}
