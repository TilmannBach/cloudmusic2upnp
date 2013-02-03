using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

using cloudmusic2upnp.ContentProvider;

namespace cloudmusic2upnp.UserInterface.Web.Protocol
{
    [DataContract]
    public class SearchResponse : Message
    {
        [DataContract]
        public class TrackData
        {
            [DataMember]
            public String Name { get; private set; }

            [DataMember]
            public String MediaUrl { get; private set; }

            public TrackData(ITrack track)
            {
                Name = track.TrackName;
                MediaUrl = track.MediaUrl;
            }
        }

        public override String ToJson()
        {
            return Header<SearchResponse>.ToJson(this);
        }

        [DataMember]
        public String Query { get; private set; }


        [DataMember]
        public TrackData[] Tracks { get; private set; }

        public SearchResponse(String query, List<ITrack> tracks)
        {
            var list = new List<TrackData>();
            foreach (var track in tracks)
            {
                list.Add(new TrackData(track));
            }

            Tracks = list.ToArray();
            Query = query;
        }
    }
}

