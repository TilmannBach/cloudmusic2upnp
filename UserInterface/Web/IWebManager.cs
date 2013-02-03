using System;
using System.IO;

using cloudmusic2upnp.UserInterface.Web.Protocol;

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

    public class ConnectionReadEventArgs : EventArgs
    {
        public IWebClient Client { get; private set; }
        public Message Message { get; private set; }

        public ConnectionReadEventArgs(IWebClient client, MemoryStream messageStream)
        {
            Client = client;
            Message = Message.FromJson(messageStream);
        }
    }

    public interface IWebManager
    {
        void Stop();

        event EventHandler<ConnectionOpenEventArgs> OnConnectionOpen;
        event EventHandler<ConnectionReadEventArgs> OnConnectionRead;
    }
}

