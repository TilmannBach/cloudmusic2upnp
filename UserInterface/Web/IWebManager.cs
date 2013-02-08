using System;
using System.IO;

using cloudmusic2upnp.UserInterface.Web.Protocol;

namespace cloudmusic2upnp.UserInterface.Web
{
    public interface IWebManager
    {
        void Start();
        void Stop();

        event Action<IWebClient> ClientConnect;
        event Action<IWebClient> ClientDisconnect;
        event Action<IWebClient, Message> ClientMessage;

    }
}

