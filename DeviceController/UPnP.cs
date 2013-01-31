using System;
using System.Collections.Generic;
using System.Xml;


namespace cloudmusic2upnp.DeviceController.UPnP
{
    public class Controller : IController
    {
        //private List<OpenHome.Net.ControlPoint.CpDevice> iDeviceList;
        /// <summary>
        /// List of devices discovered by the OpenHomeLib.
        /// </summary>
        private OpenHome.Net.ControlPoint.CpDeviceListUpnpServiceType list;

        private Dictionary<String, UPnPDevice> deviceList;
        
        OpenHome.Net.Core.Library lib;


        /// <summary>
        /// Raises if a UPnP AV media renderer is found or removed from the network.
        /// </summary>
        public event EventHandler<DeviceEventArgs> DeviceDiscovery;

        /// <summary>
        /// Starts a new DeviceController for controlling UPnP-Media-Renderer in your network.
        /// </summary>
        /// <param name="networkAdapterIndex"></param>
        public Controller(uint networkAdapterIndex = 0)
        {
            deviceList = new Dictionary<string, UPnPDevice>();

            OpenHome.Net.Core.InitParams initParams = new OpenHome.Net.Core.InitParams();
            lib = OpenHome.Net.Core.Library.Create(initParams);
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
        //~UPnP()
        //{
        //    FreeAll();
        //}

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

                XmlDocument xmlDeviceDescription = new XmlDocument();
                xmlDeviceDescription.LoadXml(deviceXml);

                if (UPnPTools.isMediaRenderer(xmlDeviceDescription))
                {
                    deviceList.Add(aDevice.Udn(), new UPnPDevice(aDevice, xmlDeviceDescription));
                    Logger.Log(Logger.Level.Debug, "Found usefull MediaRenderer: " + deviceList [aDevice.Udn()].FriendlyName);
                    OnDeviceDiscovered(deviceList [aDevice.Udn()]);
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
            String udn = aDevice.Udn();
            try
            {
                UPnPDevice dev = deviceList [udn];
                lock (deviceList)
                {
                    deviceList [udn].Free();
                    deviceList.Remove(udn);
                }
                PrintDeviceInfo("Removed", aDevice);
                OnDeviceRemoved(dev);
            } catch (KeyNotFoundException)
            {
                Logger.Log(Logger.Level.Error, "Wanted to remove a UPnP-Device from my list but it's already gone!? :/");
            }
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
        public void Shutdown()
        {
            lock (deviceList)
            {
                foreach (KeyValuePair<string, UPnPDevice> dev in deviceList)
                {
                    dev.Value.Free();
                }
                deviceList.Clear();
            }
            list.Dispose();
            Logger.Log(Logger.Level.Info, "Shutting down all UPnP bindings...");
            Logger.Log(Logger.Level.Debug, "Freeing C++ lib...");
            lib.Dispose();
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
                "\n    name = " + friendlyName
            );
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


        public IDevice GetDevice(string udn)
        {
            return deviceList [udn];
        }
    }

    public class UPnPTools
    {
        public static bool isMediaRenderer(XmlDocument xmlfile)
        {
            // Create an XmlNamespaceManager to resolve the default namespace.
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlfile.NameTable);
            nsmgr.AddNamespace("def", "urn:schemas-upnp-org:device-1-0");


            string xPathExpression = "//def:deviceType";
            var asset = (XmlElement)xmlfile.SelectSingleNode(xPathExpression, nsmgr);
            return (asset.InnerText == "urn:schemas-upnp-org:device:MediaRenderer:1") ? true : false;
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

        private OpenHome.Net.ControlPoint.Proxies.CpProxyUpnpOrgAVTransport1 avTransport;
        private OpenHome.Net.ControlPoint.Proxies.CpProxyUpnpOrgRenderingControl1 avRenderingControl;
        private XmlDocument xmlDeviceDescription;

        internal bool isStaring;

        /// <summary>
        /// Raises if the playstate of a UPnPDevice is changed.
        /// e.g. a device stopped playback because it reached the end of a song
        /// </summary>
        public event EventHandler<DevicePlaystateEventArgs> PlaystateChanged;

        public UPnPDevice(OpenHome.Net.ControlPoint.CpDevice device, XmlDocument xmlDeviceDescr)
        {
            isStaring = true;

            device.AddRef();
            iDevice = device;

            xmlDeviceDescription = xmlDeviceDescr;

            avTransport = new OpenHome.Net.ControlPoint.Proxies.CpProxyUpnpOrgAVTransport1(iDevice);
            avRenderingControl = new OpenHome.Net.ControlPoint.Proxies.CpProxyUpnpOrgRenderingControl1(iDevice);

            SubscribeToDeviceEvents();

            GetPositionInfo();
        }

        public XmlDocument GetXmlDeviceDescription()
        {
            return xmlDeviceDescription;
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

        private void GetPositionInfo()
        {
            avTransport.BeginGetPositionInfo(0, BeginGetPositionInfoComplete);
        }
        private void BeginGetPositionInfoComplete(IntPtr asyncHandle)
        {
            try
            {
                uint track;
                string trackDuration;
                string metaData;
                string trackUri;
                string relTime;
                string absTime;
                int relCount;
                int absCount;

                avTransport.EndGetPositionInfo(asyncHandle, out track, out trackDuration, out metaData, out trackUri, out relTime, out absTime, out relCount, out absCount);
            } catch (OpenHome.Net.ControlPoint.ProxyError err)
            {
                LogError(err);   
            }
            if (isStaring)
            {
                isStaring = false;
                Stop();
            }
        }

        private void LogError(OpenHome.Net.ControlPoint.ProxyError err)
        {
            Logger.Log(Logger.Level.Error, "Can't invoke an action on remote device. Device says: (" + err.Code + ") " + err.Description);
        }

        public void Play()
        {
            avTransport.BeginPlay(0, "1", BeginPlayComplete);
        }
        private void BeginPlayComplete(IntPtr asyncHandle)
        {
            try
            {
                avTransport.EndPlay(asyncHandle);
            } catch (OpenHome.Net.ControlPoint.ProxyError err)
            {
                LogError(err);
            }
        }

        public void Pause()
        {
            avTransport.BeginPause(0, BeginPauseComplete);
        }
        private void BeginPauseComplete(IntPtr asyncHandle)
        {
            try
            {
                avTransport.EndPause(asyncHandle);
            } catch (OpenHome.Net.ControlPoint.ProxyError err)
            {
                LogError(err);
            }
        }

        public void Stop()
        {
            avTransport.BeginStop(0, BeginStopComplete);
        }
        private void BeginStopComplete(IntPtr asyncHandle)
        {
            try
            {
                avTransport.EndStop(asyncHandle);
            } catch (OpenHome.Net.ControlPoint.ProxyError err)
            {
                LogError(err);
            }
        }

        public void SetMediaUrl(Uri url)
        {
            avTransport.BeginSetAVTransportURI(0, url.ToString(), " ", BeginSetMediaUrlComplete);
            //iConnection.BeginSetAVTransportURI(0, "http://dl.dropbox.com/u/22353481/temp/beer.mp3", " ", BeginSetMediaUrlComplete);
            //iConnection.BeginSetAVTransportURI(0, "http://dl.dropbox.com/u/22353481/temp/beer.mp3", "<DIDL-Lite xmlns=\"urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/\" xmlns:upnp=\"urn:schemas-upnp-org:metadata-1-0/upnp/\" xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:dlna=\"urn:schemas-dlna-org:metadata-1-0/\" xmlns:sec=\"http://www.sec.co.kr/\"><item id=\"163a411867dc2b7933a1bccd166eb310\" parentID=\"5ede10f3fc0298927d7db250d111783a\" restricted=\"1\"><upnp:class>object.item.audioItem.musicTrack</upnp:class><dc:title>Beer!!! (Album) [Explicit]</dc:title><dc:creator>Psychostick</dc:creator><upnp:artist>Psychostick</upnp:artist><upnp:albumArtURI>http://192.168.107.13:34513/MediaExport/i/MTYzYTQxMTg2N2RjMmI3OTMzYTFiY2NkMTY2ZWIzMTA%3D/th/0.jpg</upnp:albumArtURI><upnp:genre>Rock</upnp:genre><upnp:album>We Couldn't Think Of A Title [Explicit]</upnp:album><upnp:originalTrackNumber>5</upnp:originalTrackNumber><dc:date>2006-01-01</dc:date><res protocolInfo=\"http-get:*:audio/mpeg:DLNA.ORG_PN=MP3;DLNA.ORG_OP=01;DLNA.ORG_FLAGS=01700000000000000000000000000000\" bitrate=\"32000\" sampleFrequency=\"44100\" nrAudioChannels=\"2\" size=\"4533237\" duration=\"0:02:15.000\">http://192.168.107.13:34513/MediaExport/i/MTYzYTQxMTg2N2RjMmI3OTMzYTFiY2NkMTY2ZWIzMTA%3D.mp3</res></item></DIDL-Lite>", BeginSetMediaUrlComplete);
            //iConnection.BeginSetAVTransportURI(0, "http://multimediajugend.de/media/beamer/movies/BangenufProjector_LaptopMount.mp4", " ", BeginSetMediaUrlComplete);
        }
        private void BeginSetMediaUrlComplete(IntPtr asyncHandle)
        {
            avTransport.EndSetAVTransportURI(asyncHandle);
        }

        //~UPnPDevice()
        //{
        //    iDevice.RemoveRef();
        //}
        public void Free()
        {
            avRenderingControl.Unsubscribe();
            avRenderingControl.Dispose();
            avTransport.Unsubscribe();
            avTransport.Dispose();
            iDevice.RemoveRef();
        }

        public string Udn
        {
            get { return iDevice.Udn(); }
        }

        private void SubscribeToDeviceEvents()
        {
            // scheint es gar nicht zu geben !? :/
            // zyklisch GetPositionInfo
            // brauche auch noch ein Rendering-Control für mute/lautstärke
            
            // WD TV Live nimmt nur HTTPs von der soundcloud-api
            //
            avTransport.SetPropertyChanged(OnTransportPropertyChanged);
            avTransport.Subscribe();
            avRenderingControl.SetPropertyChanged(OnRenderingControlPropertyChanged);
            avRenderingControl.Subscribe();
        }

        void OnRenderingControlPropertyChanged()
        {
            Logger.Log("OnRenderingPropChanged: " + avRenderingControl.PropertyLastChange());
        }
        void OnTransportPropertyChanged()
        {
            Logger.Log("OnTransportPropChanged: " + avTransport.PropertyLastChange());
        }

        protected virtual void OnPlaystateChanged(UPnPDevice device, DevicePlaystateEventArgs.DevicePlaystate playstate, int timeOffset)
        {
            EventHandler<DevicePlaystateEventArgs> handler = PlaystateChanged;
            if (handler != null)
                handler(this, new DevicePlaystateEventArgs(device, playstate, timeOffset));
        }
    }
}