using System;
using System.Collections.Generic;
using System.Text;

namespace cloudmusic2upnp
{
	public class Core
	{
		public DeviceController.IController UPnP;
		public ContentProvider.Providers Providers;
		public UserInterface.UIProxy UI;

		/// <summary>
		/// 
		/// </summary>
		public Core ()
		{
			Logger.Log (Logger.Level.Info, "cloudmusic2upnp version " + 
				System.Reflection.Assembly.GetExecutingAssembly ().GetName ().Version.ToString () + " started"
			);

			UPnP = new DeviceController.UPnP ();
			Providers = new ContentProvider.Providers ();
			UI = new UserInterface.UIProxy (UPnP, Providers);
			UI.Start ();

			//Console.ReadLine ();
		}
	}
}
