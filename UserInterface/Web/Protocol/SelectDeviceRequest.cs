using System;
using System.Runtime.Serialization;
using System.Xml;
using System.Runtime.Serialization.Json;

using cloudmusic2upnp.ContentProvider;

namespace cloudmusic2upnp.UserInterface.Web.Protocol
{
    [DataContract]
    public class SelectDeviceRequest : Message
    {
        [DataMember]
        public string Udn;
        
        public SelectDeviceRequest()
        {
        }

        public override String ToJson()
        {
            return Header<SelectDeviceRequest>.ToJson(this);
        }
    }
}

