using System;
using System.Runtime.Serialization;

using cloudmusic2upnp.ContentProvider;
using cloudmusic2upnp.DeviceController;


namespace cloudmusic2upnp.UserInterface.Web.Protokoll
{
    [DataContract]
    public class Header
    {
        [DataMember]
        public Type Method { get; private set; }

        [DataMember]
        public Message Body { get; private set; }

        private Header()
        {
        }
    }

    [DataContract]
    public abstract class Message
    {

    }

    public class SearchRequest : Message
    {
        [DataMember]
        public String Query { get; private set; }

        [DataMember]
        public IContentProvider Provider { get; private set; }
    }

    public class SearchResponse : Message
    {
        [DataMember]
        public ITrack[] Tracks { get; private set; }
    }

    public class DeviceNotification : Message
    {
    }

    public class ProviderNotification : Message
    {
    }

    public class PlaylistNotification : Message
    {
    }

    public class PlayRequest : Message
    {
    }

    public class PlayStateNotification : Message
    {
    }


}

