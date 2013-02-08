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
        private static Playlist _active;
        public static Playlist Active
        {
            get
            {
                if (_active == null)
                {
                    _active = new Playlist();
                }
                return _active;
            }
        }


        /*
         * Properties
         */
        public List<ITrack> Items { get; private set; }


        /*
         * Methods
         */
        public Playlist()
        {
            Items = new List<ITrack>();
        }


        public void Insert(int index, ITrack item)
        {
            Items.Insert(index, item);
            if (ItemAdded != null)
                ItemAdded(item);
        }


        public void Prepend(ITrack item)
        {
            Items.Insert(0, item);
            if (ItemAdded != null)
                ItemAdded(item);
        }

        public void Append(ITrack item)
        {
            Items.Insert(Items.Count, item);
            if (ItemAdded != null)
                ItemAdded(item);
        }


        public void Remove(ITrack item)
        {
            Items.Remove(item);
            if (ItemRemoved != null)
                ItemRemoved(item);
        }


        /*
         * Events
         */
        public event Action<ITrack> ItemAdded;
        public event Action<ITrack> ItemRemoved;

    }
}

