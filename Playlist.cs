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
        public List<String> Items { get; private set; }


        /*
         * Methods
         */
        public Playlist()
        {
            Items = new List<String>();
        }


        public void Insert(int index, String item)
        {
            Items.Insert(index, item);
            if (ItemAdded != null)
                ItemAdded(item);
        }


        public void Prepend(String item)
        {
            Items.Insert(0, item);
            if (ItemAdded != null)
                ItemAdded(item);
        }

        public void Append(String item)
        {
            Items.Insert(Items.Count - 1, item);
            if (ItemAdded != null)
                ItemAdded(item);
        }


        public void Remove(String item)
        {
            Items.Remove(item);
            if (ItemRemoved != null)
                ItemRemoved(item);
        }


        /*
         * Events
         */
        public delegate void ItemHandler(String track);

        public event ItemHandler ItemAdded;
        public event ItemHandler ItemRemoved;

    }
}

