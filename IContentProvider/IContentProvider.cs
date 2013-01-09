using System;

namespace cloudmusic2upnp.IContentProvider
{
	/// <summary>
	/// The interface to the provider.
	/// </summary>
	public interface IContentProvider
	{
		String Name { get; }
		String Url { get; }

		ITrack[] Search (String term);
	}

	/// <summary>
	/// Represents a single Track of this provider.
	/// </summary>
	public interface ITrack
	{
		String TrackName { get; }

		String MediaUrl { get; }
	}


}