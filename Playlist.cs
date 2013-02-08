using System;
using System.Collections.Generic;

using cloudmusic2upnp.ContentProvider;

namespace cloudmusic2upnp
{
    public class Playlist
    {
        /*
         * Singleton 
         */
        private static ActivePlaylist _active;
        public static ActivePlaylist Active
        {
            get
            {
                if (_active == null)
                {
                    _active = new ActivePlaylist();
                }
                return _active;
            }
        }


        /*
         * Properties
         */
        public List<ITrack> Tracks { get; private set; }


        /*
         * Methods
         */
        public Playlist()
        {
            Tracks = new List<ITrack>();
        }


        public void Insert(int index, ITrack item)
        {
            Tracks.Insert(index, item);
            if (ItemAdded != null)
                ItemAdded(item);
        }


        public void Prepend(ITrack item)
        {
            Insert(0, item);
        }

        public void Append(ITrack item)
        {
            Insert(Tracks.Count, item);
        }


        public void Remove(ITrack item)
        {
            Tracks.Remove(item);
            if (ItemRemoved != null)
                ItemRemoved(item);
        }


        /*
         * Events
         */
        public event Action<ITrack> ItemAdded;
        public event Action<ITrack> ItemRemoved;

    }

    public class ActivePlaylist : Playlist
    {
        public int Index = -1;

        public ITrack Track
        {
            get
            {
                return Tracks [Index];
            }
        }

        public void PlayOrQueue(ITrack track)
        {
            Append(track);

            if (Index < 0)
            {
                Index = Tracks.Count;
                foreach (var device in Core.UPnP.GetDevices())
                {
                    device.SetMediaUrl(new Uri(track.MediaUrl));
                    device.Play();
                }
            }
        }
    }
}

