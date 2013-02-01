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
    public class Header<T>
    {
        [DataMember(Order=0)]
        public String Method { get; private set; }

        [DataMember(Name="Body", Order=1)]
        public T Message { get; private set; }

        private Header()
        {
        }

        public static String ToJson(T message)
        {
            var header = new Header<T>();
            header.Method = message.GetType().Name;
            header.Message = message;

            var ser = new DataContractJsonSerializer(typeof(Header<T>));
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
        public abstract String ToJson();
    }
}

