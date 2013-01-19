using System;
using System.Collections.Generic;
using System.Xml;


namespace cloudmusic2upnp.DeviceController
{
    public class UPnP : IController
    {
        //private List<OpenHome.Net.ControlPoint.CpDevice> iDeviceList;
        /// <summary>
        /// List of devices discovered by the OpenHomeLib.
        /// </summary>
        private OpenHome.Net.ControlPoint.CpDeviceListUpnpServiceType list;

        private Dictionary<String, UPnPDevice> deviceList;

        public event EventHandler<DeviceEventArgs> DeviceDiscovery;

        /// <summary>
        /// Starts a new DeviceController for controlling UPnP-Media-Renderer in your network.
        /// </summary>
        /// <param name="networkAdapterIndex"></param>
        public UPnP(uint networkAdapterIndex = 0)
        {
            deviceList = new Dictionary<string, UPnPDevice>();

            OpenHome.Net.Core.InitParams initParams = new OpenHome.Net.Core.InitParams();
            OpenHome.Net.Core.Library lib = OpenHome.Net.Core.Library.Create(initParams);
            OpenHome.Net.Core.SubnetList subnetList = new OpenHome.Net.Core.SubnetList();
            OpenHome.Net.Core.NetworkAdapter nif = subnetList.SubnetAt(networkAdapterIndex);
            uint subnet = nif.Subnet();
            Logger.Log(Logger.Level.Info, "Using adapter: " + nif.Name());
            subnetList.Dispose();
            lib.StartCp(subnet);

            startListening();
        }

        /// <summary>
        /// Destructor for freeing memory from c++ library.
        /// </summary>
        ~UPnP()
        {
            FreeAll();
        }

        /// <summary>
        /// Method to set up the devicelist. OpenHome-lib starts listening here...
        /// </summary>
        private void startListening()
        {
            OpenHome.Net.ControlPoint.CpDeviceList.ChangeHandler added = new OpenHome.Net.ControlPoint.CpDeviceList.ChangeHandler(DeviceAdded);
            OpenHome.Net.ControlPoint.CpDeviceList.ChangeHandler removed = new OpenHome.Net.ControlPoint.CpDeviceList.ChangeHandler(DeviceRemoved);

            //list is being discovered here, filter criteria are set
            list = new OpenHome.Net.ControlPoint.CpDeviceListUpnpServiceType("upnp.org", "ConnectionManager", 1, added, removed);
        }

        /// <summary>
        /// Handler for CpDeviceList if devices are found on the network.
        /// </summary>
        /// <param name="aList"></param>
        /// <param name="aDevice"></param>
        private void DeviceAdded(OpenHome.Net.ControlPoint.CpDeviceList aList, OpenHome.Net.ControlPoint.CpDevice aDevice)
        {
            lock (deviceList)
            {
                PrintDeviceInfo("Found", aDevice);
                
                string deviceXml;
                aDevice.GetAttribute("Upnp.DeviceXml", out deviceXml);
                if (UPnPTools.isMediaRenderer(deviceXml))
                {
                    deviceList.Add(aDevice.Udn(), new UPnPDevice(aDevice));
                    Logger.Log(Logger.Level.Debug, "Found usefull MediaRenderer: " + deviceList[aDevice.Udn()].FriendlyName);
                    OnDeviceDiscovered(deviceList[aDevice.Udn()]);
                }
            }
        }
        protected virtual void OnDeviceDiscovered(UPnPDevice dev)
        {
            EventHandler<DeviceEventArgs> handler = DeviceDiscovery;
            if (handler != null)
                handler(this, new DeviceEventArgs(dev, DeviceEventArgs.DeviceEventActions.Added));
        }

        /// <summary>
        /// Handler for CpDeviceList if devices are removed from the network (they should 
        /// send a bye if they went offline, or announcement keep alive is over).
        /// </summary>
        /// <param name="aList"></param>
        /// <param name="aDevice"></param>
        private void DeviceRemoved(OpenHome.Net.ControlPoint.CpDeviceList aList, OpenHome.Net.ControlPoint.CpDevice aDevice)
        {
            lock (deviceList)
            {
                PrintDeviceInfo("Removed", aDevice);
                deviceList.Remove(aDevice.Udn());
            }
            OnDeviceRemoved(deviceList[aDevice.Udn()]);
        }
        protected virtual void OnDeviceRemoved(UPnPDevice dev)
        {
            EventHandler<DeviceEventArgs> handler = DeviceDiscovery;
            if (handler != null)
                handler(this, new DeviceEventArgs(dev, DeviceEventArgs.DeviceEventActions.Removed));
        }

        /// <summary>
        /// Explicitly free's up memory used by the c++ library.
        /// </summary>
        void FreeAll()
        {
            list.Dispose();
            /*
            lock (deviceList)
            {
                int count = deviceList.Count;
                for (int i = 0; i < count; i++)
                {
                    deviceList[i].RemoveRef();
                }
                deviceList.RemoveRange(0, count - 1);
            }
             */
        }


        /// <summary>
        /// Prints some device information.
        /// </summary>
        /// <param name="aPrologue"></param>
        /// <param name="aDevice"></param>
        private void PrintDeviceInfo(string aPrologue, OpenHome.Net.ControlPoint.CpDevice aDevice)
        {
            string location;
            aDevice.GetAttribute("Upnp.Location", out location);
            string friendlyName;
            aDevice.GetAttribute("Upnp.FriendlyName", out friendlyName);
            Logger.Log(Logger.Level.Debug,
                aPrologue +
                "\n    udn = " + aDevice.Udn() +
                "\n    location = " + location +
                "\n    name = " + friendlyName);
        }

        public IDevice[] GetDevices()
        {
            UPnPDevice[] deviceArr;
            lock (deviceList)
            {
                deviceArr = new UPnPDevice[deviceList.Count];
                deviceList.Values.CopyTo(deviceArr, 0);
            }
            return deviceArr;
        }
    }

    public class UPnPTools
    {
        public static bool isMediaRenderer(string xmlDeviceDescription)
        {
            XmlDocument xmlfile = new XmlDocument();
            xmlfile.LoadXml(xmlDeviceDescription);

            // Create an XmlNamespaceManager to resolve the default namespace.
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlfile.NameTable);
            nsmgr.AddNamespace("def", "urn:schemas-upnp-org:device-1-0");


            string xPathExpression = "//def:deviceType";
            var asset = (XmlElement)xmlfile.SelectSingleNode(xPathExpression, nsmgr);
            if (asset.InnerText == "urn:schemas-upnp-org:device:MediaRenderer:1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class UPnPDevice : IDevice
    {
        /// <summary>
        /// CpDevice.GetAttributes only supports 
        /// • "Upnp.Location"
        /// • "Upnp.DeviceXml"
        /// • "Upnp.FriendlyName"
        /// • "Upnp.PresentationUrl"
        /// </summary>
        private OpenHome.Net.ControlPoint.CpDevice iDevice;



        public event EventHandler<DevicePlaystateEventArgs> PlaystateChanged;

        public UPnPDevice(OpenHome.Net.ControlPoint.CpDevice device)
        {
            device.AddRef();
            iDevice = device;


        }

        public string FriendlyName
        {
            get
            {
                string friendlyName;
                iDevice.GetAttribute("Upnp.FriendlyName", out friendlyName);
                return friendlyName;
            }
        }

        public void Play()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void SetMediaUrl(string url)
        {
            throw new NotImplementedException();
        }

        ~UPnPDevice()
        {
            iDevice.RemoveRef();
        }
    }
}