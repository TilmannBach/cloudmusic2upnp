using System;
using System.Net.Sockets;
using System.Collections.Generic;

using Bauglir.Ex;


namespace cloudmusic2upnp.UserInterface.Web.WebSocket
{
    public class HackedWebSocketServer : WebSocketServer
    {
        public override WebSocketServerConnection GetConnectionInstance(
		    TcpClient aClient, WebSocketHeaders aHeaders, string aHost,
			string aPort, string aResourceName, string aOrigin, string aCookie,
			string aVersion, ref string aProtocol, ref string aExtension,
		    ref int aHttpCode
        )
        {
            aProtocol = "-";
            aExtension = "-";
            return new WebSocketServerConnection(aClient, this);
        }
    }


    public class Manger : IWebManager
    {
        private HackedWebSocketServer Server;
        private int Port;
        private Dictionary<WebSocketConnection, Client> Clients;

        public event EventHandler<ClientEventArgs> ClientConnect;
        public event EventHandler<ClientEventArgs> ClientDisconnect;
        public event EventHandler<MessageEventArgs> ClientMessage;


        public Manger(int port)
        {
            Port = port;
            Clients = new Dictionary<WebSocketConnection, Client>();

            Server = new HackedWebSocketServer();
            Server.AfterAddConnection += HandleAfterAddConnection;
        }


        public void Start()
        {
            Server.Start(System.Net.IPAddress.Any, Port);
        }


        public void Stop()
        {
            Server.Stop();
        }


        private void HandleAfterAddConnection(WebSocketServer aServer, WebSocketServerConnection aConnection)
        {
            aConnection.ConnectionOpen += HandleConnectionOpen;
            aConnection.ConnectionRead += HandleConnectionRead;
            aConnection.ConnectionClose += HandleConnectionClose;
        }

        private void HandleConnectionOpen(WebSocketConnection aConnection)
        {
            var client = new Client(aConnection);
            Clients.Add(aConnection, client);
            ClientConnect(this, new ClientEventArgs(client));
        }


        private void HandleConnectionClose(WebSocketConnection aConnection, int aCloseCode, string aCloseReason, bool aClosedByPeer)
        {
            var client = Clients [aConnection];
            ClientDisconnect(this, new ClientEventArgs(client));
        }


        private void HandleConnectionRead(WebSocketConnection aConnection, bool aFinal, bool aRes1, bool aRes2, bool aRes3, int aCode, System.IO.MemoryStream aData)
        {
            if (aCode == Bauglir.Ex.WebSocketFrame.Text)
            {
                var client = Clients[aConnection];
                ClientMessage(this, new MessageEventArgs(client, aData));
            }
        }
    }
}

