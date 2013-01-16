using System;
using System.Collections.Generic;

namespace cloudmusic2upnp.ContentProvider
{
	/// <summary>
	/// The interface to the provider.
	/// </summary>
	public interface IContentProvider
	{
		String Name { get; }
		String Url { get; }

		List<ITrack> Search (String term);
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