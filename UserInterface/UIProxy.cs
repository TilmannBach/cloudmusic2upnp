using System;
using System.Collections.Generic;
using System.Threading;

namespace cloudmusic2upnp.UserInterface
{
	public interface IInterface
	{

		void Start ();

	}

	public class UIProxy
	{
		private Dictionary<string, IInterface> Interfaces;
		private Dictionary<string, Thread> Threads;

		public UIProxy (DeviceController.IController controller,
		           ContentProvider.Providers providers)
		{
			Interfaces = new Dictionary<string,IInterface> ();
			Interfaces.Add ("cli", new CLI.Interface (controller, providers));
		}

		public void Start ()
		{
			Threads = new Dictionary<string,Thread> ();

			foreach (KeyValuePair<string,IInterface> i in Interfaces) {
				Thread t = new Thread (i.Value.Start);
				Threads.Add (i.Key, t);
				t.Start ();
			}
		}
	}
}

