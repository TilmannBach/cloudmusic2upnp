using System;
using Bauglir.Ex;

namespace cloudmusic2upnp.UserInterface.Web.WebSocket
{
    public class Client : IWebClient
    {
        private WebSocketConnection Connection;

        public Client(WebSocketConnection aConnection)
        {
            Connection = aConnection;
        }

        public void SendMessage(Protocol.Message message)
        {
            String json = message.ToJson();
            Connection.SendText(json);
        }
    }
}

