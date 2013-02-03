using System;
using System.IO;

using cloudmusic2upnp.UserInterface.Web.Protocol;

namespace cloudmusic2upnp.UserInterface.Web
{
    public class ClientConnectEventArgs : EventArgs
    {
        public IWebClient Client { get; private set; }

        public ClientConnectEventArgs(IWebClient client)
        {
            Client = client;
        }
    }

    public class ClientMessageEventArgs : EventArgs
    {
        public IWebClient Client { get; private set; }
        public Message Message { get; private set; }

        public ClientMessageEventArgs(IWebClient client, MemoryStream messageStream)
        {
            Client = client;
            Message = Message.FromJson(messageStream);
        }
    }

    public interface IWebManager
    {
        void Start();
        void Stop();

        event EventHandler<ClientConnectEventArgs> ClientConnect;
        event EventHandler<ClientMessageEventArgs> ClientMessage;
    }
}

