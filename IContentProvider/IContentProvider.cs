using System;

namespace cloudmusic2upnp.IContentProvider
{
	abstract public class Track
	{
		public String TrackName { get; protected set; }

		public String MediaUrl { get; protected set; }
	}

	public interface IContentProvider
	{
		Track[] Search (String term);
	}

}