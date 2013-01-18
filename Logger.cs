using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cloudmusic2upnp
{
	public class Logger
	{
		public enum Level
		{
			Debug,
			Info,
			Warning,
			Error
		}

		public static void Log (String message)
		{
			Log (Level.Debug, message);
		}

		public static void Log (Level level, String message)
		{
			Console.WriteLine ("{0} [{1}] - {2}", DateTime.Now, level, message);
		}
	}
}
