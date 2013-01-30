using System;

namespace cloudmusic2upnp.UserInterface.Web
{
    public class ConnectionOpenEventArgs : EventArgs
    {
        public IWebClient Client { get; private set; }

        public ConnectionOpenEventArgs(IWebClient client)
        {
            Client = client;
        }
    }

    public interface IWebManager
    {
        void Stop();

        event EventHandler<ConnectionOpenEventArgs> OnConnectionOpen;
    }
}

