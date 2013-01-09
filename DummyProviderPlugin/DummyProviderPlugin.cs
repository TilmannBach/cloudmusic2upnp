using System;

using cloudmusic2upnp.IContentProvider;

namespace DummyProviderPlugin
{
	public class DummyTrack : Track
	{
		public DummyTrack ()
		{
			this.TrackName = "Dummy Track";
			this.MediaUrl = "http://dl.dropbox.com/u/22353481/temp/beer.mp3";
		}
	}

	public class DummyProvider : IContentProvider
	{
		public DummyProvider ()
		{
		}

		public Track[] Search (String term)
		{
			return new Track[] {new DummyTrack ()};
		}
	}
}

