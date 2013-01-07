using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSource.UPnP;

namespace cloudmusic2upnp
{
    class Program
    {
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
    }
}
