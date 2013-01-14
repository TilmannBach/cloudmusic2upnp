using System;

using cloudmusic2upnp.ContentProvider;

namespace cloudmusic2upnp.ContentProvider.Plugins.Soundcloud
{
	public class Track : ITrack
	{
		public String TrackName {
			get {
				return "";
			}
		}

		public String MediaUrl {
			get {
				return "";
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
				return "Soundcloud";
			}
		}

		public String Url {
			get {
				return "https://soundcloud.com/";
			}
		}

		public ITrack[] Search (String term)
		{
			return new ITrack[] {new Track ()};
		}
	}
}

