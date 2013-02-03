using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace cloudmusic2upnp.Utils
{
	public class Logger
	{
		[Flags]
		public enum Outputs
		{
			Quiet = 0x00,
			Console = 0x01,
			File = 0x02,
            Both = 0x03
		}

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
            Config cfg = Config.Load();

			if (cfg.LogVerbosity <= level) {
				string line = String.Format ("{0} [{1}] - {2}", DateTime.Now, level, message);

				if ((cfg.LogOutput & Outputs.Console) == Outputs.Console) {
                    if (level <= Level.Info)
                    {
                        Console.WriteLine(line);
                    }
                    else
                    {
                        Console.Error.WriteLine(line);
                    }
				}

				if ((cfg.LogOutput & Outputs.File) == Outputs.File) {
                    File.AppendAllText(cfg.LogFile, line + Environment.NewLine);
				}
			}
		}
	}
}
