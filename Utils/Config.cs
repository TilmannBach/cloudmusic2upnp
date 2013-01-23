using System;
using System.IO;
using System.Xml.Serialization;

namespace cloudmusic2upnp.Utils
{
	public class Config
	{
		public Logger.Outputs LogOutput;
		public Logger.Level LogVerbosity;
		public String LogFile;

		private const string Path = "config.xml";
		private static Config Instance;

		private Config ()
		{
			LogOutput = Logger.Outputs.Quiet;
			LogVerbosity = Logger.Level.Info;
			LogFile = "cloudmusic2upnp.log";
		}

		public static Config Load ()
		{
			if (Instance == null) {
				try {
					XmlSerializer ser = new XmlSerializer (typeof(Config));
					StreamReader sr = new StreamReader (Path);
					Instance = (Config)ser.Deserialize (sr);
					sr.Close ();
				} catch (FileNotFoundException) {
					Instance = new Config ();
				}
			}

			Instance.Save ();
			return Instance;
		}

		public void Save ()
		{
			XmlSerializer ser = new XmlSerializer (typeof(Config));
			FileStream str = new FileStream (Path, FileMode.Create);
			ser.Serialize (str, this);
			str.Close ();
		}
	}
}

