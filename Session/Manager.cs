using System;

namespace cloudmusic2upnp.Session
{
    public class Manager
    {
        private static Playlist _playlist;
        public static Playlist Playlist
        {
            get
            {
                if (_playlist == null)
                {
                    _playlist = new Playlist();
                }
                return _playlist;
            }
        }


        private Manager()
        {
        }
    }
}

