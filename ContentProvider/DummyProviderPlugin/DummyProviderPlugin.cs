using System;
using System.Collections.Generic;

using cloudmusic2upnp.ContentProvider;

namespace cloudmusic2upnp.ContentProvider.Plugins.Dummy
{
    public class Track : ITrack
    {
        public String Name
        {
            get
            {
                return "Dummy Track";
            }
        }

        public String MediaUrl
        {
            get
            {
                return "http://dl.dropbox.com/u/22353481/temp/beer.mp3";
            }
        }

        public String ID
        {
            get
            {
                return "Dummy:beer";
            }
        }
    }

    public class Provider : IContentProvider
    {
        public Provider()
        {
        }

        public String Name
        {
            get
            {
                return "Dummy";
            }
        }

        public String Url
        {
            get
            {
                return "https://github.com/TilmannBach/cloudmusic2upnp";
            }
        }

        public List<ITrack> Search(String term)
        {
            var l = new List<ITrack>();
            l.Add(new Track());
            return l;
        }

        public ITrack GetById(String ID)
        {
            return new Track();
        }
    }
}

