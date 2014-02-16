using System;
using System.Collections.Generic;

using cloudmusic2upnp.ContentProvider;
using System.Runtime.Serialization;

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
        public List<PlayListItem> Tracks { get; private set; }


        /*
         * Methods
         */
        public Playlist()
        {
            Tracks = new List<PlayListItem>();
        }


        public void Insert(int index, ITrack track)
        {
            PlayListItem item = new PlayListItem(track);
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


        public void Remove(int playListID)
        {
            PlayListItem item = Tracks.Find(obj => obj.PlayListID == playListID);
            Tracks.RemoveAt(Tracks.FindIndex(obj => obj.PlayListID == playListID));
            if (ItemRemoved != null)
                ItemRemoved(item);
        }


        /*
         * Events
         */
        public event Action<PlayListItem> ItemAdded;
        public event Action<PlayListItem> ItemRemoved;

    }

    public class ActivePlaylist : Playlist
    {
        /// <summary>
        /// Index of the playing song starts with 0!
        /// </summary>
        public int Index = -1;

        public PlayListItem CurrentTrack
        {
            get
            {
                return Tracks[Index];
            }
        }

        private void Play()
        {
            foreach (var device in Core.UPnP.GetDevices())
            {
                device.SetMediaUrl(new Uri(CurrentTrack.Track.MediaUrl));
                device.Play();
            }
        }

        public void PlayNext()
        {
            Index++;
            if (Index <= Tracks.Count)
                Play();
        }

        public void PlayOrQueue(ITrack track)
        {
            Append(track);

            if (Index < 0)
            {
                Index = 0;
                Play();
            }
        }

        internal void AddDeviceController(DeviceController.IController DeviceController)
        {
            DeviceController.DeviceDiscovery += HandleDeviceDiscovery;
        }

        void HandleDeviceDiscovery(object sender, DeviceController.DeviceEventArgs e)
        {
            e.Device.PlaystateChanged += handlePlaystateChanged;
        }

        void handlePlaystateChanged(object sender, DeviceController.DevicePlaystateEventArgs e)
        {
            if (e.Playstate == DeviceController.DevicePlaystateEventArgs.DevicePlaystate.ReachedEnd)
            {
                PlayNext();
            }
        }
    }

    [DataContract]
    [KnownType(typeof(ContentProvider.Plugins.Soundcloud.Track))]
    public class PlayListItem
    {
        [DataMember]
        public int PlayListID;

        [DataMember]
        public ITrack Track;

        //[DataMember]
        //public bool IsActive;

        public PlayListItem(ITrack track)
        {
            PlayListID = this.GetHashCode();
            Track = track;
            //IsActive = false;
        }
    }
}

