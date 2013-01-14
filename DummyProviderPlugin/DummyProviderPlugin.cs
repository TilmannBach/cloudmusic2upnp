using System;

using cloudmusic2upnp.ContentProvider;

namespace cloudmusic2upnp.ContentProvider.Plugins.Dummy
{
	public class Track : ITrack
	{
		public String TrackName {
			get {
				return "Dummy Track";
			}
		}

		public String MediaUrl {
			get {
				return "http://dl.dropbox.com/u/22353481/temp/beer.mp3";
			}
		}
	}

	public class Provider : IContentProvider
	{
		public Provider ()
		{
		}

		public String Name {
			get {
				return "Dummy";
			}
		}

		public String Url {
			get {
				return "https://github.com/TilmannBach/cloudmusic2upnp";
			}
		}

		public ITrack[] Search (String term)
		{
			return new ITrack[] {new Track ()};
		}
	}
}

