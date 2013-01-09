using System;

using cloudmusic2upnp.IContentProvider;

namespace DummyProviderPlugin
{
	public class DummyTrack : ITrack
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

	public class DummyProvider : IContentProvider
	{
		public DummyProvider ()
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
			return new ITrack[] {new DummyTrack ()};
		}
	}
}

