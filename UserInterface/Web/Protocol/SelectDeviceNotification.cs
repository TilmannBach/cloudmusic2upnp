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
    public class SelectDeviceNotification : Message
    {
        [DataMember]
        public readonly string Udn;

        public SelectDeviceNotification(IDevice device)
        {
            Udn = device.Udn;
        }

        public override String ToJson()
        {
            return Header<SelectDeviceNotification>.ToJson(this);
        }
    }
}

