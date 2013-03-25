using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

using cloudmusic2upnp.ContentProvider;

namespace cloudmusic2upnp.UserInterface.Web.Protocol
{
    [DataContract]
    [KnownType(typeof(ContentProvider.Plugins.Soundcloud.Track))]
    public class SearchResponse : Message
    {
        public override String ToJson()
        {
            return Header<SearchResponse>.ToJson(this);
        }

        [DataMember]
        public String Query { get; private set; }

        [DataMember]
        public ITrack[] Tracks { get; private set; }

        public SearchResponse(String query, List<ITrack> tracks)
        {
            Tracks = tracks.ToArray();
            Query = query;
        }
    }
}

