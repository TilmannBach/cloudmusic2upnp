using System;

namespace cloudmusic2upnp.UserInterface.Web
{
    public interface IWebClient
    {
        void SendMessage(Protokoll.Message message);
    }
}

