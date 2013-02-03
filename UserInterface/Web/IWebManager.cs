using System;
using System.IO;

using cloudmusic2upnp.UserInterface.Web.Protocol;

namespace cloudmusic2upnp.UserInterface.Web
{
    public class ClientEventArgs : EventArgs
    {
        public IWebClient Client { get; private set; }

        public ClientEventArgs(IWebClient client)
        {
            Client = client;
        }
    }

    public class MessageEventArgs : EventArgs
    {
        public IWebClient Client { get; private set; }
        public Message Message { get; private set; }

        public MessageEventArgs(IWebClient client, MemoryStream messageStream)
        {
            Client = client;
            Message = Message.FromJson(messageStream);
        }
    }

    public interface IWebManager
    {
        void Start();
        void Stop();

        event EventHandler<ClientEventArgs> ClientConnect;
        event EventHandler<ClientEventArgs> ClientDisconnect;
        event EventHandler<MessageEventArgs> ClientMessage;
    }
}

