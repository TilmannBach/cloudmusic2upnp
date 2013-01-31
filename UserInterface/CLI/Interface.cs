using System;

namespace cloudmusic2upnp.UserInterface.CLI
{
    public class Interface : IInterface
    {
        private DeviceController.IController Controller;
        private ContentProvider.Providers Providers;
        private uint defaultPlayer = 0;

        public event EventHandler InterfaceShutdownRequest;

        public Interface(DeviceController.IController controller,
                              ContentProvider.Providers providers)
        {
            Controller = controller;
            //   Controller.DeviceDiscovery += Controller_DeviceDiscovery;
            Providers = providers;
        }

        public void Start()
        {
            CmdHelp();

            while (true)
            {
                string cmd = Console.ReadLine();

                switch (cmd)
                {

                    case "help":
                        CmdHelp();
                        break;

                    case "search":
                        CmdSearch();
                        break;

                    case "exit":
                        EventHandler handler = InterfaceShutdownRequest;
                        if (handler != null)
                            handler(this, EventArgs.Empty);
                        return;

                    case "set":
                        Controller.GetDevices()[defaultPlayer].SetMediaUrl(new Uri(""));
                        break;
                    case "play":
                        Controller.GetDevices()[defaultPlayer].Play();
                        break;
                    case "pause":
                        Controller.GetDevices()[defaultPlayer].Pause();
                        break;
                    case "stop":
                        Controller.GetDevices()[defaultPlayer].Stop();
                        break;
                    case "player":
                        Console.WriteLine("Available players: ");
                        foreach (DeviceController.IDevice dev in Controller.GetDevices())
                        {
                            Console.WriteLine(" - " + dev.FriendlyName);
                        }
                        break;
                    case "player 0":
                        defaultPlayer = 0;
                        break;
                    case "player 1":
                        defaultPlayer = 1;
                        break;

                    default:
                        Console.WriteLine("Unknown command.");
                        CmdHelp();
                        break;
                }
            }
        }


        private void CmdHelp()
        {
            Console.WriteLine("Avalible commands:");
            Console.WriteLine("  help - this help");
            Console.WriteLine("  search - search for a title");
            Console.WriteLine("  exit - exits this program");
            Console.WriteLine("");

        }

        private void CmdSearch()
        {
            Console.Write("Term: ");
            string query = Console.ReadLine();

            // TODO: easify Providers interface
            var tracks = Providers.Plugins["Soundcloud"].Search(query);

            foreach (var t in tracks)
            {
                Console.WriteLine("  Soundcloud: {0}", t.TrackName);
            }
        }

        public void Stop()
        { }
    }
}