using System;
using System.Net;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

using cloudmusic2upnp.ContentProvider;

namespace cloudmusic2upnp.ContentProvider.Plugins.Soundcloud
{
	public class Track : ITrack
	{
		public Track (JObject json)
		{
			_TrackName = (String)json ["title"];
			_MediaUrl = (String)json ["stream_url"];
		}

		private String _TrackName;
		private String _MediaUrl;

		public String TrackName {
			get {
				return _TrackName;
			}
		}

		public String MediaUrl {
			get {
				return _MediaUrl;
			}
		}
	}

	/// <summary>
	/// Provider.
	/// </summary>
	/// <example>
	/// List<ITrack> tracks = ContentProvider.Providers.Instance.AllPlugins ["Soundcloud"].Search ("flume");
	/// </example>
	public class Provider : IContentProvider
	{

		public String Name {
			get {
				return "Soundcloud";
			}
		}

		public String Url {
			get {
				return "http://soundcloud.com/";
			}
		}

		private const String API_KEY = "apigee";
		private const String API_URL = "http://api.soundcloud.com/";
		private const String API_FORMAT = "json";

		public Provider ()
		{
			IssueOverrideProxy ();
		}

		/// <summary>
		/// This is a workaround for Mono < 2.11 on Linux with enabled
		/// proxy, because it will hang up, if there is a 'http_proxy'
		/// environment variable set.
		/// </summary>
		private void IssueOverrideProxy ()
		{
			WebRequest.GetSystemWebProxy ();
		}



		public List<ITrack> Search (String term)
		{
			List<ITrack> tracks = new List<ITrack> ();

			JArray array = ApiRequest ("tracks", "q", term);
			foreach (JObject jobj in array) {
				tracks.Add (new Track (jobj));
			}

			return tracks;
		}


		private JArray ApiRequest (String method, params String[] filters)
		{
			if ((filters.Length % 2) != 0)
				// Throw an exception, if the filters aren't a modulo of 2,
				// because it alternates between key and value. I don't like
				// it neither.
				throw new ArgumentException ("Filters must have key and value");

			String url = API_URL + method + "." + API_FORMAT + "?consumer_key=" + API_KEY;

			for (var i=0; i<filters.Length; i=i+2) {
				url += "&" + filters [i];
				url += "=" + filters [i + 1];
			}

			var request = HttpWebRequest.Create (url);
			WebResponse response = request.GetResponse ();
			StreamReader reader = new StreamReader (response.GetResponseStream ());
			String json = reader.ReadToEnd ();

			return JArray.Parse (json);
		}
	}
}

