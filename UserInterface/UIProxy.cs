using System;
using System.Collections.Generic;
using System.Threading;

namespace cloudmusic2upnp.UserInterface
{
    public interface IInterface
    {
        event EventHandler InterfaceShutdownRequest;
        void Start();
        void Stop();
    }

    public class UIProxy
    {
        private Dictionary<string, IInterface> Interfaces;
        private Dictionary<string, Thread> Threads;

        public event EventHandler UIShutdownRequest;

        public UIProxy(DeviceController.IController controller,
                   ContentProvider.Providers providers)
        {
            Interfaces = new Dictionary<string, IInterface>();
            Interfaces.Add("cli", new CLI.Interface(controller, providers));
            Interfaces.Add("web", new Web.Interface(controller, providers));
        }

        public void Start()
        {
            Threads = new Dictionary<string, Thread>();

            foreach (KeyValuePair<string, IInterface> i in Interfaces)
            {
                i.Value.InterfaceShutdownRequest += OnInterfaceShutdownRequest;
                Thread t = new Thread(i.Value.Start);
                Threads.Add(i.Key, t);
                t.Start();
            }
        }

        public void Stop()
        {
            foreach (KeyValuePair<string, IInterface> i in Interfaces)
            {
                i.Value.Stop();
            }
        }

        void OnInterfaceShutdownRequest(object sender, EventArgs e)
        {
            EventHandler handler = UIShutdownRequest;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}