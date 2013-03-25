using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

using cloudmusic2upnp.ContentProvider;

namespace cloudmusic2upnp.UserInterface.Web.Protocol
{
    [DataContract]
    [KnownType(typeof(ContentProvider.Plugins.Soundcloud.Track))]
    public class PlaylistNotification : Message
    {
        [DataMember]
        public ITrack[]
            Tracks;

        public PlaylistNotification(Playlist playlist)
        {
            Tracks = playlist.Tracks.ToArray();
        }

        public override String ToJson()
        {
            return Header<PlaylistNotification>.ToJson(this);
        }
    }
}

