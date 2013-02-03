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
            public readonly String
                Name;

            [DataMember]
            public readonly String
                MediaUrl;

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
        public readonly String
            Query;

        [DataMember]
        public readonly TrackData[]
            Tracks;

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

