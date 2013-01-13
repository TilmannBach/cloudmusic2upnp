using System;

using cloudmusic2upnp.IContentProvider;

namespace SoundcloudProviderPlugin
{
	public class SoundcloudProviderPlugin : IContentProvider
	{
		public String Name{ get; private set; }

		public String Url { get; private set; }


		public class Track : ITrack
		{
			public String TrackName { get; private set; }

			public String MediaUrl { get; private set; }
		}


		public SoundcloudProviderPlugin ()
		{
		}

		public ITrack[] Search (String term)
		{
			throw NotImplementedException;
		}
	}
}

