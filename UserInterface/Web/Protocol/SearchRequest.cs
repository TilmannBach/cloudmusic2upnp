using System;
using System.Runtime.Serialization;
using System.Xml;
using System.Runtime.Serialization.Json;


namespace cloudmusic2upnp.UserInterface.Web.Protocol
{
    [DataContract]
    public class SearchRequest : Message
    {
        [DataMember]
        public String
            Query;

        public SearchRequest()
        {

        }

        public override String ToJson()
        {
            return Header<SearchRequest>.ToJson(this);
        }
    }
}

