using System;

namespace cloudmusic2upnp.UserInterface.CLI
{
	public class Interface : IInterface
	{
		private DeviceController.IController Controller;
		private ContentProvider.Providers Providers;

		public Interface (DeviceController.IController controller,
		                      ContentProvider.Providers providers)
		{
			Controller = controller;
			Providers = providers;
		}

		public void Start ()
		{
			CmdHelp ();

			while (true) {
				string cmd = Console.ReadLine ();

				switch (cmd) {

				case "help":
					CmdHelp ();
					return;

				case "search":
					CmdSearch ();
					return;

				case "exit":
					Console.WriteLine ("Bye!");
					return;

				default:
					Console.WriteLine ("Unknown command.");
					CmdHelp ();
					break;
				}

			}
		}


		private void CmdHelp ()
		{
			Console.WriteLine ("Avalible commands:");
			Console.WriteLine ("  help - this help");
			Console.WriteLine ("  search - search for a title");
			Console.WriteLine ("  exit - exits this program");
			Console.WriteLine ("");

		}

		private void CmdSearch ()
		{
			Console.Write ("Term: ");
			string query = Console.ReadLine ();

			// TODO: easify Providers interface
			var tracks = Providers.AllPlugins ["Soundcloud"].Search (query);

			foreach (var t in tracks) {
				Console.WriteLine ("  Soundcloud: {0}", t.TrackName);
			}
		}
	}
}

