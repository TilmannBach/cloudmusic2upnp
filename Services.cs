using System;

namespace cloudmusic2upnp.Services
{
	abstract class Track
	{
		public String TrackName { get; private set; }

		public String MediaUrl { get; private set; }
	}

	interface IService
	{
		Track[] Search (String term);
	}
}

