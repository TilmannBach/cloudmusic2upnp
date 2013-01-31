using System;
using System.IO;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;

using cloudmusic2upnp.ContentProvider;
using cloudmusic2upnp.DeviceController;


namespace cloudmusic2upnp.UserInterface.Web.Protocol
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

        public static String ToJson(Message message)
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
        public String ToJson()
        {
            return Header.ToJson(this);
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


    [DataContract]
    public class DeviceNotification : Message
    {
        [DataContract]
        public class DeviceData
        {
            [DataMember]
            public readonly string
                Udn;

            [DataMember]
            public readonly string
                Name;

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
    }


    [DataContract]
    public class ProviderNotification : Message
    {
        [DataContract]
        public class ProviderData
        {
            [DataMember]
            public readonly string
                ID;

            [DataMember]
            public readonly string
                Name;

            public ProviderData(IContentProvider provider)
            {
                ID = provider.Name;
                Name = provider.Name;
            }
        }

        [DataMember]
        public readonly ProviderData[]
            Providers;


        public ProviderNotification(Providers providers)
        {
            var list = new List<ProviderData>();
            foreach (KeyValuePair<string, IContentProvider> kvp in providers.Plugins)
            {
                list.Add(new ProviderData(kvp.Value));
            }

            Providers = list.ToArray();
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

