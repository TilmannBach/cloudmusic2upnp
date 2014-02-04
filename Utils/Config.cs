using System;
using System.IO;
using System.Xml.Serialization;

namespace cloudmusic2upnp.Utils
{
	public class Config
	{
		public Utils.Logger.Outputs LogOutput;
		public Utils.Logger.Level LogVerbosity;
		public String LogFile;
		public int HttpPort = 80;
        public uint LanInterfaceId = 0;

        private const string Path = "config.xml";
		private static Config Instance;

		private Config ()
		{
			LogOutput = Utils.Logger.Outputs.Console;
			LogVerbosity = Utils.Logger.Level.Info;
			LogFile = "cloudmusic2upnp.log";
		}

		public static Config Load ()
		{
			if (Instance == null) {
				try {
                    Path.Insert(0, AppDomain.CurrentDomain.BaseDirectory);
					XmlSerializer ser = new XmlSerializer (typeof(Config));
                    StreamReader sr = new StreamReader(Path);
					Instance = (Config)ser.Deserialize (sr);
					sr.Close ();
				} catch (FileNotFoundException) {
					Instance = new Config ();
				}
			}

			//Instance.Save ();
			return Instance;
		}

		public static void Save ()
		{
			XmlSerializer ser = new XmlSerializer (typeof(Config));
			FileStream str = new FileStream (Path, FileMode.Create);
			ser.Serialize (str, Config.Load());
			str.Close ();
		}
	}
}

