using System;
using System.Collections.Generic;


namespace cloudmusic2upnp
{
    public class UPnPDeviceController : IDeviceController
    {
        private bool iListFrozen;
        private List<OpenHome.Net.ControlPoint.CpDevice> iDeviceList;
        /// <summary>
        /// List of devices discovered by the OpenHomeLib.
        /// </summary>
        private OpenHome.Net.ControlPoint.CpDeviceListUpnpServiceType list;

        /// <summary>
        /// Starts a new DeviceController for controlling UPnP-Media-Renderer in your network.
        /// </summary>
        public UPnPDeviceController(uint networkAdapterIndex = 0)
        {
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
        ~UPnPDeviceController()
        {
            FreeAll();
        }

        /// <summary>
        /// Method to set up the devicelist. OpenHome-lib starts listening here...
        /// </summary>
        private void startListening()
        {
            iListFrozen = false;
            iDeviceList = new List<OpenHome.Net.ControlPoint.CpDevice>();
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
            lock (this)
            {
                if (!iListFrozen)
                {
                    PrintDeviceInfo("Added", aDevice);
                    aDevice.AddRef();
                    iDeviceList.Add(aDevice);
                }
            }
        }

        /// <summary>
        /// Handler for CpDeviceList if devices are removed from the network (they should 
        /// send a bye if they went offline, or announcement keep alive is over).
        /// </summary>
        /// <param name="aList"></param>
        /// <param name="aDevice"></param>
        private void DeviceRemoved(OpenHome.Net.ControlPoint.CpDeviceList aList, OpenHome.Net.ControlPoint.CpDevice aDevice)
        {
            lock (this)
            {
                if (!iListFrozen)
                {
                    PrintDeviceInfo("Removed", aDevice);
                    string udn = aDevice.Udn();
                    int count = iDeviceList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (iDeviceList[i].Udn() == udn)
                        {
                            iDeviceList[i].RemoveRef();
                            iDeviceList.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Explicitly free's up memory used by the c++ library.
        /// </summary>
        void FreeAll()
        {
            lock (iDeviceList)
            {
                int count = iDeviceList.Count;
                for (int i = 0; i < count; i++)
                {
                    iDeviceList[i].RemoveRef();
                }
                iDeviceList.RemoveRange(0, count - 1);
            }
            list.Dispose();
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
            Logger.Log(Logger.Level.Info,
                aPrologue +
                "\n    udn = " + aDevice.Udn() +
                "\n    location = " + location +
                "\n    name = " + friendlyName + "\n");
        }

        public IDevice[] GetDevices()
        {
            throw new NotImplementedException();
        }
    }

    public class UPnPDevice : IDevice
    {
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
    }
}
