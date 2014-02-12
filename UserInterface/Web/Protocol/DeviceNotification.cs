using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Runtime.Serialization.Json;

using cloudmusic2upnp.ContentProvider;
using cloudmusic2upnp.DeviceController;

namespace cloudmusic2upnp.UserInterface.Web.Protocol
{
    [DataContract]
    public class DeviceNotification : Message
    {
        [DataContract]
        public class DeviceData
        {
            [DataMember]
            public string Udn { get; private set; }

            [DataMember]
            public string Name { get; private set; }

            public DeviceData(IDevice Device)
            {
                Name = Device.FriendlyName;
                Udn = Device.Udn;
            }
        }

        [DataMember]
        public readonly DeviceData[]
            Devices;

        public DeviceNotification(IController controller)
        {
            var list = new List<DeviceData>();
            foreach (IDevice device in controller.GetDevices())
            {
                list.Add(new DeviceData(device));
            }
            Devices = list.ToArray();
        }

        public override String ToJson()
        {
            return Header<DeviceNotification>.ToJson(this);
        }
    }
}

