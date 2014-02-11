using System;
using System.Runtime.Serialization;
using System.Xml;
using System.Runtime.Serialization.Json;

using cloudmusic2upnp.ContentProvider;

namespace cloudmusic2upnp.UserInterface.Web.Protocol
{
    [DataContract]
    public class SetMuteRequest : Message
    {
        [DataMember]
        public Boolean
            SetMuted;
        
        public SetMuteRequest()
        {
        }

        public override String ToJson()
        {
            return Header<SetMuteRequest>.ToJson(this);
        }
    }
}

