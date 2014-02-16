using System;
using System.Runtime.Serialization;
using System.Xml;
using System.Runtime.Serialization.Json;


namespace cloudmusic2upnp.UserInterface.Web.Protocol
{
    [DataContract]
    public class PlaylistRemoveItemRequest : Message
    {
        [DataMember]
        public int PlaylistItem;

        public PlaylistRemoveItemRequest()
        {

        }

        public override String ToJson()
        {
            return Header<PlaylistRemoveItemRequest>.ToJson(this);
        }
    }
}

