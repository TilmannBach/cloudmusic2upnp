using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cloudmusic2upnp
{
    public class UPnP
    {
        /// <summary>
        /// Dictionary for all devices found. Hash is DeviceUdn.
        /// </summary>
        private Dictionary<string, Mono.Upnp.Device> myFoundDevices;


        public UPnP()
        {
            myFoundDevices = new Dictionary<string, Mono.Upnp.Device>();

            Mono.Upnp.Client controlPoint = new Mono.Upnp.Client();
            controlPoint.DeviceAdded += new EventHandler<Mono.Upnp.DeviceEventArgs>(controlPoint_DeviceAdded);
            controlPoint.ServiceAdded += new EventHandler<Mono.Upnp.ServiceEventArgs>(controlPoint_ServiceAdded);

            controlPoint.BrowseAll();
        }

        void controlPoint_ServiceAdded(object sender, Mono.Upnp.ServiceEventArgs e)
        {
            throw new NotImplementedException();
        }

        void controlPoint_DeviceAdded(object sender, Mono.Upnp.DeviceEventArgs e)
        {
            myFoundDevices.Add(e.Device.Udn, e.Device.GetDevice());
            EventLogger.Log("Found new device! Its friendly name is: " + e.Device.GetDevice().FriendlyName);
            Console.WriteLine("Found new device! Its friendly name is: {0}", e.Device.GetDevice().FriendlyName);
        }
    }
}
