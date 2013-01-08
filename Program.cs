using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// using OpenSource.UPnP;
// using Mono.Upnp;

namespace cloudmusic2upnp
{
    class Program
    {
        /// <summary>
        /// Dictionary for all devices found. Hash is DeviceUdn.
        /// </summary>
        private Dictionary<string, Mono.Upnp.Device> myFoundDevices;

        /// <summary>
        /// Implementation test for Mono.Upnp
        /// </summary>
        /// <param name="args">none</param>
        static void Main(string[] args)
        {
            Program myProgram = new Program();
        }

        Program()
        {
            myFoundDevices = new Dictionary<string, Mono.Upnp.Device>();

            Mono.Upnp.Client controlPoint = new Mono.Upnp.Client();
            controlPoint.DeviceAdded += new EventHandler<Mono.Upnp.DeviceEventArgs>(controlPoint_DeviceAdded);
            controlPoint.ServiceAdded += new EventHandler<Mono.Upnp.ServiceEventArgs>(controlPoint_ServiceAdded);

            controlPoint.BrowseAll();

            Console.ReadLine();
            return;
        }

        void controlPoint_ServiceAdded(object sender, Mono.Upnp.ServiceEventArgs e)
        {
            throw new NotImplementedException();
        }

        void controlPoint_DeviceAdded(object sender, Mono.Upnp.DeviceEventArgs e)
        {
            myFoundDevices.Add(e.Device.Udn, e.Device.GetDevice());
            Console.WriteLine("Found new device! Its friendly name is: {0}", e.Device.GetDevice().FriendlyName);
        }


        /*
         * 
         * 
         * 

        /// <summary>
        /// Implementation test for OpenSource.UPnP
        /// </summary>
        /// <param name="args">none</param>
        static void Main(string[] args)
        {
            UPnPSmartControlPoint controlPoint = new UPnPSmartControlPoint();
            controlPoint.OnAddedDevice += new UPnPSmartControlPoint.DeviceHandler(controlPoint_OnAddedDevice);
            controlPoint.OnAddedService += new UPnPSmartControlPoint.ServiceHandler(controlPoint_OnAddedService);
            controlPoint.Rescan();

            Console.ReadLine();
            return;
        }

        static void controlPoint_OnAddedService(UPnPSmartControlPoint sender, UPnPService service)
        {
            throw new NotImplementedException();
        }

        static void controlPoint_OnAddedDevice(UPnPSmartControlPoint sender, UPnPDevice device)
        {
            Console.WriteLine("Device found, yeah! It's friendly name is: {0}", device.FriendlyName);
            foreach (UPnPService service in device.Services)
            {
                Console.WriteLine("-> Service found on device: {0}", service.ServiceURN);
            }
            return;
        }
         * 
         */
    }
}