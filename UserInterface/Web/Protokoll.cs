using System;
using System.IO;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;

using cloudmusic2upnp.ContentProvider;
using cloudmusic2upnp.DeviceController;


namespace cloudmusic2upnp.UserInterface.Web.Protokoll
{
    [DataContract]
    public class Header
    {
        [DataMember(Order=0)]
        public String Method { get; private set; }

        [DataMember(Name="Body", Order=1)]
        public Message Message { get; private set; }

        private Header()
        {
        }

        public static String ToJSON(Message message)
        {
            var header = new Header();
            header.Method = message.GetType().Name;
            header.Message = message;

            var ser = new DataContractJsonSerializer(typeof(Header));
            var s = new MemoryStream();
            ser.WriteObject(s, header);
            s.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(s);
            return reader.ReadToEnd();
        }
    }

    [DataContract]
    public abstract class Message
    {
        public String ToJSON()
        {
            return Header.ToJSON(this);
        }
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

    [DataContract]
    public class ProviderNotification : Message
    {
        private Providers Providers;
        [DataMember]
        public Dictionary<String,String> ProviderIDs
        {
            get
            {
                var ids = new Dictionary<String,String>();
                foreach (KeyValuePair<string, IContentProvider> kvp in Providers.AllPlugins)
                {
                    ids.Add(kvp.Key, kvp.Value.Name);
                }
                return ids;
            }
            private set {}
        }


        public ProviderNotification(Providers providers)
        {
 Providers = providers;
        }
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

