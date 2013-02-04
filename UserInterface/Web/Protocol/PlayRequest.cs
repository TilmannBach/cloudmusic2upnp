using System;
using System.Runtime.Serialization;
using System.Xml;
using System.Runtime.Serialization.Json;

namespace cloudmusic2upnp.UserInterface.Web.Protocol
{
    [DataContract]
    public class PlayRequest : Message
    {
        [DataMember]
        public String
            MediaUrl;

        public PlayRequest()
        {
        }

        public override String ToJson()
        {
            return Header<PlayRequest>.ToJson(this);
        }
    }
}

